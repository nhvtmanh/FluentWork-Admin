let dataTable;
let editor;

$(document).ready(function () {
    LoadDataTable();
    FilterDataTable();
    HandleChangeTypeDropdown();
});

function FilterDataTable() {
    $('#lessonType, #lessonTopic, #lessonLevel').on('change', function () {
        LoadDataTable();
    });
}

//Handle change Type dropdown
function HandleChangeTypeDropdown() {
    $('#lessonType').change(function () {
        let selectedType = $(this).val();

        $.ajax({
            url: '/Lesson/GetTopicsByType',
            type: 'GET',
            data: { type: selectedType },
            success: function (topics) {
                let $topicDropdown = $('#lessonTopic');
                $topicDropdown.empty();
                $topicDropdown.append('<option value="">All</option>');

                $.each(topics, function (i, topic) {
                    $topicDropdown.append($('<option>', {
                        value: topic.value,
                        text: topic.text
                    }));
                });

                LoadDataTable();
            },
            error: function () {
                ShowToastNoti('error', 'Failed to load topics.');
            }
        });
    });
}

function LoadDataTable() {
    if ($.fn.DataTable.isDataTable('#dataTable')) {
        dataTable.ajax.reload();
        return;
    }

    dataTable = $('#dataTable').DataTable({
        order: [[1, 'asc']],
        ajax: {
            url: "/Lesson/GetList",
            type: "GET",
            data: function (d) {
                d.type = $('#lessonType').val();

                let topic = $('#lessonTopic').val();

                d.vocabularyTopic = d.type === 'Vocabulary' ? topic : null;
                d.grammarTopic = d.type === 'Grammar' ? topic : null;

                d.level = $('#lessonLevel').val();
            },
            datatype: "json",
            dataSrc: function (response) {
                return response.data;
            },
            error: function (err) {
                return [];
            }
        },
        columns: [
            {
                data: "id",
                orderable: false,
                render: function (data) {
                    return RenderActionButtons(data);
                }
            },
            {
                data: "title",
                className: "align-middle"
            },
            {
                data: "type",
                render: function (data) {
                    return RenderEnglishType(data);
                },
                className: "text-center align-middle"
            },
            {
                data: "null",
                render: function (data, type, row) {
                    if (row.type === 'Vocabulary')
                        return row.vocabulary_topic;
                    else
                        return row.grammar_topic;
                },
                className: "text-center align-middle"
            },
            {
                data: "level",
                render: function (data) {
                    return RenderEnglishLevel(data);
                },
                className: "text-center align-middle"
            }
        ]
    });
}

function ShowAddModal() {
    $("#lessonModalContent").load('/Lesson/P_AddOrEdit', function () {
        $("#lessonModal").modal('show');
        InitCkeditor();
    });
}

function InitCkeditor() {
    const {
        ClassicEditor,
        Essentials,
        Bold,
        Italic,
        Font,
        Paragraph
    } = CKEDITOR;

    ClassicEditor
        .create(document.querySelector('#Content'), {
            licenseKey: 'eyJhbGciOiJFUzI1NiJ9.eyJleHAiOjE3NDgwNDQ3OTksImp0aSI6IjZkOTY0NGU2LWE3ZjAtNDE0ZC1iYzRhLWRhMmYxMjdlZWQ1MCIsInVzYWdlRW5kcG9pbnQiOiJodHRwczovL3Byb3h5LWV2ZW50LmNrZWRpdG9yLmNvbSIsImRpc3RyaWJ1dGlvbkNoYW5uZWwiOlsiY2xvdWQiLCJkcnVwYWwiLCJzaCJdLCJ3aGl0ZUxhYmVsIjp0cnVlLCJsaWNlbnNlVHlwZSI6InRyaWFsIiwiZmVhdHVyZXMiOlsiKiJdLCJ2YyI6IjAyNmU4ODRlIn0.5YvRgupp6IucLyEvXGpCXlEo0hQa6JeeNDxLNfzip-upki1mObHzG382XICRr_NioYMDytNY3_nk31LASB8lTA',
            plugins: [Essentials, Bold, Italic, Font, Paragraph],
            toolbar: [
                'undo', 'redo', '|', 'bold', 'italic', '|',
                'fontSize', 'fontFamily', 'fontColor', 'fontBackgroundColor'
            ]
        })
        .then(newEditor => {
            editor = newEditor;
        })
        .catch(/* ... */);
}

function ShowEditModal(id) {
    $("#lessonModalContent").load(`/Lesson/P_AddOrEdit?id=${id}`, function () {
        $("#lessonModal").modal('show');
        InitCkeditor();

        //Display Topic dropdown
        ToggleTopicDropdown();
    });
}

function ShowDeleteModal(id) {
    swal.fire({
        title: 'Are you sure you want to delete this item?',
        text: `You won't be able to revert this action!`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: "Delete",
        cancelButtonText: "Cancel",
        customClass: {
            confirmButton: "btn btn-danger mx-1",
            cancelButton: "btn btn-outline-secondary mx-1"
        },
        buttonsStyling: false,
        showLoaderOnConfirm: true,
        preConfirm: () => {
            return $.ajax({
                url: '/Lesson/Delete',
                type: 'DELETE',
                data: { id: id },
                dataType: 'json',
                success: function (response) {
                    ShowToastNoti('success', response.message);
                    dataTable.ajax.reload();
                },
                error: function (err) {
                    //Handle other errors (e.g., server errors)
                    ShowToastNoti('error', 'An error occurred, please try again.');
                }
            });
        }
    });
}

function Submit() {
    let form = $("#lessonForm");

    if (!form.valid()) {
        return;
    }

     //Sync CKEditor content to the textarea before submit
    let content = editor.getData();
    $('textarea#Content').val(content);

    let formData = new FormData(form[0]);
    formData.append("__RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());

    $.ajax({
        url: "/Lesson/P_AddOrEdit",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            ShowToastNoti('success', response.message);
            $("#lessonModal").modal("hide");
            dataTable.ajax.reload();
        },
        error: function (err) {
            if (err.status === 400) {
                let errorMessages = err.responseJSON.message;
                errorMessages.forEach(function (message) {
                    ShowToastNoti('error', message);
                })
            } else {
                //Handle other errors (e.g., server errors)
                ShowToastNoti('error', 'An error occurred, please try again.');
            }
        }
    });
}