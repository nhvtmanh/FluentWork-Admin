// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Show toastr notification
function ShowToastNoti(type, message) {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": true,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    toastr[type](message); //type = success, error, info, warning
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

//Render action buttons in DataTable
function RenderActionButtons(id) {
    let html = `<div class="d-flex justify-content-center align-items-center">
                    <button class="btn btn-success btn-circle mx-1" onclick="ShowEditModal(${id})">
                        <i class="fas fa-pen"></i>
                    </button>
                    <button class="btn btn-danger btn-circle mx-1" onclick="ShowDeleteModal(${id})">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>`;
    return html;
}

//Render English Type in DataTable
function RenderEnglishType(type) {
    let html = '';
    if (type === 'Vocabulary')
        html = `<span class="badge p-2" style="background-color: #DED0F2">${type}</span>`;
    else
        html = `<span class="badge p-2" style="background-color: #D4F2D0">${type}</span>`;
    return html;
}

//Render English Level in DataTable
function RenderEnglishLevel(level) {
    let html = '';
    if (level === 'Beginner')
        html = `<span class="badge p-2" style="background-color: #D6EEFC">${level}</span>`;
    else if (level === 'Intermediate')
        html = `<span class="badge p-2" style="background-color: #FFDEBD">${level}</span>`;
    else
        html = `<span class="badge p-2" style="background-color: #FFDBEA">${level}</span>`;
    return html;
}

//Render Vocabulary topic in DataTable
function RenderVocabularyTopic(topic) {
    let html = '';
    if (topic === 'Information Technology') {
        html = `<span class="badge p-2" style="background-color: #CBEBF7">${topic}</span>`;
    } else if (topic === 'Business') {
        html = `<span class="badge p-2" style="background-color: #DCF7DE">${topic}</span>`;
    } else if (topic === 'Finance') {
        html = `<span class="badge p-2" style="background-color: #FFFEDB">${topic}</span>`;
    }
    return html;
}

//Show account modal
function ShowAccountModal(id) {
    $("#accountModalContent").load(`/Account/P_EditAccount?id=${id}`, function () {
        $("#accountModal").modal('show');
    });
}

//Submit account modal
function SubmitAccountModal() {
    let form = $("#profileForm");

    if (!form.valid()) {
        return;
    }

    let formData = new FormData(form[0]);
    formData.append("__RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());

    $.ajax({
        url: "/Account/P_EditAccount",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            ShowToastNoti('success', response.message);
            $("#accountModal").modal("hide");
            $("#span_username").text(response.data.username);
        },
        error: function (err) {
            if (err.status === 400) {
                let errorMessages = err.responseJSON.message;
                errorMessages.forEach(function (message) {
                    ShowToastNoti('warning', message);
                })
            } else {
                //Handle other errors (e.g., server errors)
                ShowToastNoti('error', 'An error occurred, please try again.');
            }
        }
    });
}

function Logout() {
    $.ajax({
        url: "/Account/Logout",
        type: "GET",
        contentType: false,
        processData: false,
        success: function (response) {
            ShowToastNoti('success', response.message);

            setTimeout(function () {
                window.location.href = '/admin/login';
            }, 1000);
        },
        error: function (err) {
            if (err.status === 400 || err.status === 404) {
                let errorMessages = err.responseJSON.message;
                errorMessages.forEach(function (message) {
                    ShowToastNoti('warning', message);
                })
            } else {
                //Handle other errors (e.g., server errors)
                ShowToastNoti('error', 'An error occurred, please try again.');
            }
        }
    });
}