$(document).ready(function () {
    var t = $('#table').DataTable({
        "columnDefs": [
            {
                "searchable": false,
                "orderable": false,
                "targets": [0,-1]
            },
        /*{ "type": "date", "targets": 3 }*/
        ],
        "order": [[4, "desc"]],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.21/i18n/Polish.json"
        }
        
    });

    t.on('order.dt search.dt', function () {
        t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});