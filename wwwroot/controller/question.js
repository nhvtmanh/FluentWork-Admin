let dataTable;

$(document).ready(function () {
    LoadDataTable();
    FilterDataTable();
    HandleChangeTypeDropdown();
});

function FilterDataTable() {
    $('#questionType, #questionTopic, #questionLevel').on('change', function () {
        LoadDataTable();
    });
}

//Handle change Type dropdown
function HandleChangeTypeDropdown() {
    $('#questionType').change(function () {
        let selectedType = $(this).val();

        $.ajax({
            url: '/Question/GetTopicsByType',
            type: 'GET',
            data: { type: selectedType },
            success: function (topics) {
                let $topicDropdown = $('#questionTopic');
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
            url: "/Question/GetList",
            type: "GET",
            data: function (d) {
                d.type = $('#questionType').val();

                let topic = $('#questionTopic').val();

                d.vocabularyTopic = d.type === 'Vocabulary' ? topic : null;
                d.grammarTopic = d.type === 'Grammar' ? topic : null;

                d.level = $('#questionLevel').val();
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
                data: "question_text",
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
    $("#questionModalContent").load('/Question/P_AddOrEdit', function () {
        $("#questionModal").modal('show');
    });
}

function ShowEditModal(id) {
    $("#questionModalContent").load(`/Question/P_AddOrEdit?id=${id}`, function () {
        $("#questionModal").modal('show');

        //Display Topic dropdown
        ToggleTopicDropdown();
    });
}

function Submit() {
    let form = $("#questionForm");

    if (!form.valid()) {
        return;
    }

    let formData = new FormData(form[0]);
    formData.append("__RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());

    $.ajax({
        type: "POST",
        url: "/Question/P_AddOrEdit",
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            ShowToastNoti('success', response.message);
            $("#questionModal").modal("hide");
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