let dataTable;

$(document).ready(function () {
    LoadDataTable();
    FilterDataTable();
});

function FilterDataTable() {
    $('#selectRole').on('change', function () {
        LoadDataTable();
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
            url: "/User/GetList",
            type: "GET",
            data: function (d) {
                d.role = $('#selectRole').val();
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
                data: "username",
                className: "align-middle"
            },
            {
                data: "email",
                className: "align-middle"
            },
            {
                data: "fullname",
                className: "align-middle"
            },
            {
                data: "role",
                render: function (data) {
                    return RenderUserRole(data);
                },
                className: "text-center align-middle"
            },
            {
                data: "createdAt",
                render: (data, type, row, meta) => data ? moment(data).format('HH:mm DD-MM-YYYY') : '',
                className: "align-middle"
            },
            {
                data: "updatedAt",
                render: (data, type, row, meta) => data ? moment(data).format('HH:mm DD-MM-YYYY') : '',
                className: "align-middle"
            }
        ],
        initComplete: function () {
            //Delete current account row
            const idToDelete = $('#accountId').val();

            if (idToDelete) {
                dataTable.rows(function (idx, data, node) {
                    return data.id == idToDelete;
                }).remove().draw();
            }
        }
    });
}

function RenderUserRole(role) {
    let html = '';
    if (role === 'Admin')
        html = `<span class="badge p-2" style="background-color: #FFCFC8">${role}</span>`;
    else if (role ==='Learner')
        html = `<span class="badge p-2" style="background-color: #D0F9ED">${role}</span>`;
    else if (role === 'Instructor')
        html = `<span class="badge p-2" style="background-color: #E1D7FB">${role}</span>`;
    return html;
}

function ShowAddModal() {
    $("#userModalContent").load('/User/P_AddOrEdit', function () {
        $("#userModal").modal('show');
    });
}

function ShowEditModal(id) {
    $("#userModalContent").load(`/User/P_AddOrEdit?id=${id}`, function () {
        $("#userModal").modal('show');
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
                url: '/User/Delete',
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
    let form = $("#userForm");

    if (!form.valid()) {
        return;
    }

    let formData = new FormData(form[0]);
    formData.append("__RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());

    $.ajax({
        url: "/User/P_AddOrEdit",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            ShowToastNoti('success', response.message);
            $("#userModal").modal("hide");
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