// Call the dataTables jQuery plugin
$(document).ready(function () {
    LoadDataTable();

    //'use strict';
    //const forms = document.querySelectorAll('.needs-validation');
    //Array.from(forms).forEach(function (form) {
    //    form.addEventListener('submit', function (event) {
    //        if (!form.checkValidity()) {
    //            event.preventDefault();
    //            event.stopPropagation();
    //        }
    //        form.classList.add('was-validated');
    //    }, false);
    //});

});

//Change Topic Dropdown based on Type selection
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
    dataTable = $('#dataTable').DataTable({
        ajax: {
            url: "/Question/GetList",
            type: "GET",
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
                render: function (data) {
                    return buttonActionHtml(data);
                }
            },
            {
                data: "questionText",
                className: "align-middle",
            },
            {
                data: "type",
                render: function (data) {
                    return typeHtml(data);
                },
                className: "text-center align-middle"
            },
            {
                data: "null",
                render: function (data, type, row) {
                    if (row.type === 'Vocabulary')
                        return row.vocabularyTopic;
                    else
                        return row.grammarTopic;
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
            if (response.status === 200) {
                dataTable.ajax.reload();
                $("#questionModal").modal('hide');
            } else {
                // Show error
            }
        },
        error: function () {
            // Show error
        }
    });
}