$(function () {
    
    // по нажатию на кнопку происходит фильтрация дисков 
    $('#buttonSearch').on('click', function () {
        

        var valueDirection = $('#filterDirection').val();
        var valueSinger = $('#filterSinger').val();


        console.log(valueDirection + '<- direction vivod__singer ->' + valueSinger);
        
        $.ajax({
            url: '/Disks/Filter',
            data: { singer: valueSinger, direction: valueDirection },
            type: 'GET',
            success: function (data) {
                $('#table_cont').html(data);
                //$('#table_cont').html('----');
            }

        })
    })

    $(document).delegate('#rating', 'click', function (e) {
        
        
        var i = $(e.target).data('id');
        var p = $(e.target).parents('tr').data('disk-id');
        var str = "";

        $.ajax({
            url: '/Disks/Rate',
            data: { rating: parseInt(i), disk: parseInt(p) },
            type: 'POST',
            success: function (data) {
                if (data == 0) {
                    str = "Вы не оценили этот CD";
                } else {
                    str = data;
                }

                

                $('[data-disk-id=' + p + ']').find('[data-tot-rat]').text(data);
                $('[data-disk-id='+ p +']').find('[data-your-rat]').text(i);

                //$(e.target).closest('[data-tot-rat]').text(data);
                //$(e.target).parents('tr').children('[data-your-rat]').text(i);

                //console.log($(e.target).parents('tr').children('[tot-rat]').text());

            }
        })
        console.log(i + " ==== " + p);
    })

    $(document).delegate('[data-remove-disk]', 'click', function () {

        var diskId = $(this).data('remove-disk');
        var el = $(this);

        console.log(diskId);

        $.ajax({
            url: '/Disks/DeleteDisk',
            data: { diskId: parseInt(diskId) },
            type: 'POST',
            success: function (data) {
                el.parents('tr').fadeOut();
            }

        })
    })


    $('#filterDirection').autocomplete({
        source: '/Disks/AutoCompleteDirection', classes: {
            "ui-autocomplete": "highlight"
        }
    });

    $('#filterSinger').autocomplete({
        source: '/Disks/AutoCompleteSinger', classes: {
            "ui-autocomplete": "highlight"
        }
    });



});