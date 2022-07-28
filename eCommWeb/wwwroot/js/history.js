$(document).ready(function () {
    $('#DT_load').DataTable({
        "ajax": {
            "url": "/history/get",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name" },
            {"data": "photo",
                "render": function (data, type, full, meta) {
                    return '<img src="/images/' + data + '" height="70px" width="70px"/>';  
                }
            },
            { "data": "price" },
            { "data": "quantity" },
            { "data": "subTotal" },
            { "data": "purchaseDate" }

        ],

        "width": "100%"

    });
});