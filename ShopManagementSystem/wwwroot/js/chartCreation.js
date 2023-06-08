document.addEventListener('DOMContentLoaded', function () {

    var chartData = JSON.parse(document.getElementById('chartData').getAttribute('data-chartData'));

    var ctx = document.getElementById('chart').getContext('2d');
    ctx.canvas.width = 800;
    ctx.canvas.height = 500;
    var chart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: chartData.labels,
            datasets: [{
                label: 'Support',
                data: chartData.supportData,
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1
            }, {
                label: 'Confidence',
                data: chartData.confidenceData,
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderColor: 'rgba(255, 99, 132, 1)',
                borderWidth: 1
            }]
        },
        options: {
            responsive: false,
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });

    var productChartData = JSON.parse(document.getElementById('productChartData').getAttribute('data-chartData'));
   
    var ctx = document.getElementById('productChart').getContext('2d');
    ctx.canvas.width = 800;
    ctx.canvas.height = 500;
    var productChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: productChartData.labels,
            datasets: [{
                label: 'Total Number Of Products Sold',
                data: productChartData.productData,
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1
            }]
        },
        options: {
            responsive: false,
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });

    var totalSalesChartData = JSON.parse(document.getElementById('totalSalesChartData').getAttribute('data-chartData'));
   
    var ctx = document.getElementById('totalSalesChart').getContext('2d');
    ctx.canvas.width = 800;
    ctx.canvas.height = 500;
    var totalSalesChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: totalSalesChartData.labels,
            datasets: [{
                label: 'Total Sales Per Day',
                data: totalSalesChartData.totalSalesData,
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1,
                fill: false
            }]
        },
        options: {
            responsive: false,
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });

});