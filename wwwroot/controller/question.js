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

//Change Topic dropdown based on Type selection
function ToggleTopicDropdown() {
    const selectedType = $("select#Type").val();

    const vocabularyTopicGroup = $("#vocabularyTopicGroup")
    const grammarTopicGroup = $("#grammarTopicGroup");

    const vocabularyDropdown = $("#VocabularyTopic");
    const grammarDropdown = $("#GrammarTopic");

    if (selectedType === "Vocabulary") {
        vocabularyTopicGroup.show();
        grammarTopicGroup.hide();
        grammarDropdown.val(""); //Clear Grammar dropdown
    } else if (selectedType === "Grammar") {
        vocabularyTopicGroup.hide();
        grammarTopicGroup.show();
        vocabularyDropdown.val(""); //Clear Vocabulary dropdown
    } else {
        vocabularyTopicGroup.hide();
        grammarTopicGroup.hide();
        vocabularyDropdown.val("");
        grammarDropdown.val("");
    }
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
                d.topic = $('#questionType').val();

                let topic = $('#questionTopic').val();

                d.vocabularyTopic = d.topic === 'Vocabulary' ? topic : null;
                d.grammarTopic = d.topic === 'Grammar' ? topic : null;

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
                    return buttonActionHtml(data);
                }
            },
            {
                data: "question_text",
                className: "align-middle"
            },
            {
                data: "topic",
                render: function (data) {
                    return typeHtml(data);
                },
                className: "text-center align-middle"
            },
            {
                data: "null",
                render: function (data, type, row) {
                    if (row.topic === 'Vocabulary')
                        return row.vocabulary_topic;
                    else
                        return row.grammar_topic;
                },
                className: "text-center align-middle"
            },
            {
                data: "level",
                render: function (data) {
                    return levelHtml(data);
                },
                className: "text-center align-middle"
            }
        ]
    });
}

const buttonActionHtml = function (id) {
    let html = ``;
    html += `<button class="btn btn-success btn-circle mx-1" onclick="ShowModalEdit(${id})">
                <i class="fas fa-pen"></i>
            </button>`;
    html += `<button class="btn btn-danger btn-circle" onclick="ShowModalDelete(${id})">
                <i class="fas fa-trash"></i>
            </button>`
    return html;
}

const typeHtml = function (type) {
    let html = '';
    if (type === 'Vocabulary')
        html = `<span class="badge p-2" style="background-color: #DED0F2">${type}</span>`;
    else
        html = `<span class="badge p-2" style="background-color: #D4F2D0">${type}</span>`;
    return html;
}

const levelHtml = function (level) {
    let html = '';
    if (level === 'Beginner')
        html = `<span class="badge p-2" style="background-color: #D6EEFC">${level}</span>`;
    else if (level === 'Intermediate')
        html = `<span class="badge p-2" style="background-color: #FFDEBD">${level}</span>`;
    else
        html = `<span class="badge p-2" style="background-color: #FFDBEA">${level}</span>`;
    return html;
}

function ShowAddModal() {
    $("#questionModalContent").load('/Question/P_AddOrEdit', function () {
        $("#questionModal").modal('show');
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