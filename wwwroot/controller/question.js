// Call the dataTables jQuery plugin
$(document).ready(function () {
    LoadDataTable();

    $('#btnCreate').click(function () {
        $("#createQuestionModalContent").load('/Question/P_Add', function () {
            $("#createModal").modal('show');
        });
    });
});

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
                data: "questionText"
            },
            {
                data: "type",
                render: function (data) {
                    return typeHtml(data);
                },
                className: "text-center"
            },
            {
                data: "topic",
                className: "text-center"
            },
            {
                data: "level",
                render: function (data) {
                    return levelHtml(data);
                },
                className: "text-center"
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