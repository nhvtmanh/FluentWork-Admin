$(document).ready(function () {
    LoadSummary();
    LoadTotalLearnersChart();
    LoadRolesChart();
})

function LoadSummary() {
    $.ajax({
        url: "/Dashboard/GetSummary",
        type: "GET",
        contentType: false,
        processData: false,
        success: function (response) {
            $('#divTotalLessons').text(response.data.totalLessons);
            $('#divTotalQuestions').text(response.data.totalQuestions);
            $('#divTotalFlashcards').text(response.data.totalFlashcards);
            $('#divTotalUsers').text(response.data.totalUsers);
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

function LoadTotalLearnersChart() {
    $.ajax({
        url: "/Dashboard/GetLearnersDaily",
        type: "GET",
        contentType: false,
        processData: false,
        success: function (response) {
            LoadAreaChart(response.data);
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

function LoadAreaChart(data) {
    const groupedByMonth = {};

    data.forEach(item => {
        const date = new Date(item.date);
        const month = `${date.getMonth() + 1}-${date.getFullYear()}`; // "M-YYYY"

        if (!groupedByMonth[month]) {
            groupedByMonth[month] = 0;
        }
        groupedByMonth[month] += item.count;
    });

    const listLabels = Object.keys(groupedByMonth);
    const listData = Object.values(groupedByMonth);

    // Set new default font family and font color to mimic Bootstrap's default styling
    Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
    Chart.defaults.global.defaultFontColor = '#858796';

    var ctx = document.getElementById("learnersAreaChart");
    if (ctx) {
        var myLineChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: listLabels,
                datasets: [{
                    label: "Total",
                    lineTension: 0.3,
                    backgroundColor: "rgba(78, 115, 223, 0.05)",
                    borderColor: "rgba(78, 115, 223, 1)",
                    pointRadius: 3,
                    pointBackgroundColor: "rgba(78, 115, 223, 1)",
                    pointBorderColor: "rgba(78, 115, 223, 1)",
                    pointHoverRadius: 3,
                    pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
                    pointHoverBorderColor: "rgba(78, 115, 223, 1)",
                    pointHitRadius: 10,
                    pointBorderWidth: 2,
                    data: listData,
                }],
            },
            options: {
                maintainAspectRatio: false,
                layout: {
                    padding: {
                        left: 10,
                        right: 25,
                        top: 25,
                        bottom: 0
                    }
                },
                scales: {
                    xAxes: [{
                        time: {
                            unit: 'date'
                        },
                        gridLines: {
                            display: false,
                            drawBorder: false
                        },
                        ticks: {
                            maxTicksLimit: 12
                        }
                    }],
                    yAxes: [{
                        ticks: {
                            maxTicksLimit: 5,
                            padding: 10
                        },
                        gridLines: {
                            color: "rgb(234, 236, 244)",
                            zeroLineColor: "rgb(234, 236, 244)",
                            drawBorder: false,
                            borderDash: [2],
                            zeroLineBorderDash: [2]
                        }
                    }],
                },
                legend: {
                    display: false
                },
                tooltips: {
                    backgroundColor: "rgb(255,255,255)",
                    bodyFontColor: "#858796",
                    titleMarginBottom: 10,
                    titleFontColor: '#6e707e',
                    titleFontSize: 14,
                    borderColor: '#dddfeb',
                    borderWidth: 1,
                    xPadding: 15,
                    yPadding: 15,
                    displayColors: false,
                    intersect: false,
                    mode: 'index',
                    caretPadding: 10
                }
            }
        });
    }
}

function LoadRolesChart() {
    $.ajax({
        url: "/Dashboard/GetRoles",
        type: "GET",
        contentType: false,
        processData: false,
        success: function (response) {
            LoadPieChart(response.data);
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

function LoadPieChart(data) {
    const order = ["Learner", "Instructor", "Admin"];
    const listData = data
        .sort((a, b) => order.indexOf(a.role) - order.indexOf(b.role))
        .map(item => item.count);

    // Set new default font family and font color to mimic Bootstrap's default styling
    Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
    Chart.defaults.global.defaultFontColor = '#858796';

    var ctx = document.getElementById("rolesPieChart");
    if (ctx) {
        var myPieChart = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: order,
                datasets: [{
                    data: listData,
                    backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc'],
                    hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf'],
                    hoverBorderColor: "rgba(234, 236, 244, 1)",
                }],
            },
            options: {
                maintainAspectRatio: false,
                tooltips: {
                    backgroundColor: "rgb(255,255,255)",
                    bodyFontColor: "#858796",
                    borderColor: '#dddfeb',
                    borderWidth: 1,
                    xPadding: 15,
                    yPadding: 15,
                    displayColors: false,
                    caretPadding: 10,
                },
                legend: {
                    display: false
                },
                cutoutPercentage: 80,
            },
        });
    }
}