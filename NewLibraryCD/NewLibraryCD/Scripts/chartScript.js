$(function ($) {

    getArrValue(0);


    //получаем ид выбранного элемента
    $(document).delegate('#buttonViewChart', 'click', function () {
        var dir = $('#selectedDirection').val();
        getArrValue(parseInt(dir));
    });
});

// получаем массив с данными
function getArrValue(selectDirectionId) {

    var dirName = '';

    if (isNaN(selectDirectionId) || selectDirectionId==0) {
        selectDirectionId = 0
        dirName = 'Все категории'

    } else {
        dirName = $('#selectedDirection option:selected').text();
        console.log('ne nan ^ ' + dirName);
    }
    

    $.ajax({
        url: '/Disks/GetArrValue',
        data: { directionId: selectDirectionId},
        type: 'POST',
        success: function (data) {
            init(data, dirName);
        }

    });
}

function init(data, dir) {

    //console.log(dir);

    Highcharts.chart('container', {
        chart: {
            type: 'area'
        },
        title: {
            text: 'График зависимости музыкального направления: ' + dir
        },
        subtitle: {
            text: 'Source: <a href="http://thebulletin.metapress.com/content/c4120650912x74k7/fulltext.pdf">' +
                'thebulletin.metapress.com</a>'
        },
        xAxis: {
            allowDecimals: false,
            labels: {
                formatter: function () {
                    return this.value; // clean, unformatted number for year
                }
            }
        },
        yAxis: {
            max: 10,
            title: {
                text: 'Оценка'
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            }
        },
        tooltip: {
            pointFormat: '{series.name} популярен на <b>{point.y:,.0f}</b><br/> баллов в {point.x}' + 'году'
        },
        plotOptions: {
            area: {
                pointStart: 1950,
                marker: {
                    enabled: false,
                    symbol: 'circle',
                    radius: 2,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                }
            }
        },
        series: [{
            name: dir,
            data:data
            //name: 'USSR/Russia',
            //data: [null, null, null, null, null, null, null, null, null, null,
            //    5, 7, 50, 120, 150, 200, 426, 660, 869, 1060, 1605, 2471, 3322,
            //    4238, 5221, 6129, 7089, 8339, 9399, 10538, 11643, 13092, 14478,
            //    15915, 17385, 19055, 21205, 23044, 25393, 27935, 30062, 32049,
            //    33952, 35804, 37431, 39197, 45000, 43000, 41000, 39000, 37000,
            //    35000, 33000, 31000, 29000, 27000, 25000, 24000, 23000, 22000,
            //    21000, 20000, 19000, 18000, 18000, 17000, 16000]
        }]
    });
}