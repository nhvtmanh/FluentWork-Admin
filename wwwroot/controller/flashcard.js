let dataTable;

$(document).ready(function () {
    LoadDataTable();
    FilterDataTable();
});

function FilterDataTable() {
    $('#flashcardTopic').on('change', function () {
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
            url: "/Flashcard/GetList",
            type: "GET",
            data: function (d) {
                d.topic = $('#flashcardTopic').val();
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
                data: "word",
                className: "align-middle"
            },
            {
                data: "definition",
                className: "align-middle"
            },
            {
                data: "topic",
                render: function (data) {
                    return RenderVocabularyTopic(data);
                },
                className: "text-center align-middle"
            }
        ]
    });
}

function ShowAddModal() {
    $("#flashcardModalContent").load('/Flashcard/P_AddOrEdit', function () {
        $("#flashcardModal").modal('show');
    });
}

function ShowEditModal(id) {
    $("#flashcardModalContent").load(`/Flashcard/P_AddOrEdit?id=${id}`, function () {
        $("#flashcardModal").modal('show');
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
                url: '/Flashcard/Delete',
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
    let form = $("#flashcardForm");

    if (!form.valid()) {
        return;
    }

    let formData = new FormData(form[0]);
    formData.append("__RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());

    $.ajax({
        url: "/Flashcard/P_AddOrEdit",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            ShowToastNoti('success', response.message);
            $("#flashcardModal").modal("hide");
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