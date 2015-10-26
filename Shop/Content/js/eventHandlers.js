$("#EditCartDialog").dialog({
    autoOpen: false,
    maxWidth: '80%',
    maxHeight: 'auto',
    width: '80%',
    height: 'auto',
    modal: true
});

$("#Cart").ready(function() {
    $.ajax({
        url: '/Cart/CountSkuOnCart',
        success: function (data) {
            $('#Cart').html(data);
        }
    });
});


function editCartClick(parameters) {
    $("#EditCartDialog").dialog("open");
}



 

