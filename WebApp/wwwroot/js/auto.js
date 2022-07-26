$(document).ready(function () {
    $("#Name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Product/Company",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.name, value: item.name };
                    }));

                }
            })
        },
        messages: {
            noResults: "",
            results: function (count) {
                return count + (count > 1 ? ' wyniki' : ' wynik') + ' znaleziono';
            }
        }
    });
})