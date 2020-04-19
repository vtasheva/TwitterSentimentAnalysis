$(document).ready(function () {
    $('#searchForm').submit(function (e) {
        if ($(this).valid()) {
            $('.validation-summary-errors').remove();
        }
    });
});

function renderChart(data) {
    var chart = new CanvasJS.Chart("chartContainer", {
        theme: "light2", // "light1", "light2", "dark1", "dark2"
        animationEnabled: true,
        title: {
            fontSize: 21
        },
        data: [{
            type: "pie",
            startAngle: 160,
            toolTipContent: "<b>{label}</b>: {y}%",
            indexLabel: "{label} - {y}%",
            dataPoints: data
        }]
    });
    chart.render();
}