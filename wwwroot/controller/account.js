function Login() {
    let form = $("#loginForm");

    if (!form.valid()) {
        return;
    }

    let formData = new FormData(form[0]);
    formData.append("__RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());

    $.ajax({
        url: "/Account/Login",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            ShowToastNoti('success', response.message);

            setTimeout(function () {
                window.location.href = '/admin/dashboard';
            }, 1000);
        },
        error: function (err) {
            if (err.status === 400 || err.status === 401 || err.status === 404) {
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

function ForgotPassword() {
    let form = $("#forgotPasswordForm");

    if (!form.valid()) {
        return;
    }

    let formData = new FormData(form[0]);
    formData.append("__RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());

    $.ajax({
        url: "/Account/ForgotPassword",
        type: "POST",
        data: formData,
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