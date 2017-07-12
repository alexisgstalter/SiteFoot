$(document).ready(function () {
    Load();
    function Load() {
        $.ajax({
            url: "GetUtilisateurs",
            type: "POST",
            data: "{}",
            dataType: "json",
            success: function (data) {
                if (data.ok) {
                    $("#user_container").append(data.html);
                }
                else {
                    Materialize.toast(data.error, 3000);
                }


            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);
            }
        });
    }
})