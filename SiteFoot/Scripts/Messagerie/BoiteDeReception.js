$(document).ready(function () {
    load();
    
    function load() {
        $('#mail_container').empty();
        $.ajax({
            url: '/Messagerie/GetAllMessages',
            dataType: 'json',
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({  }),
            success: function (data) {
                $('#mail_container').append(data.html);
                $(".table").dataTable();
                $("select").material_select();
            }
        });
    }
    var current_id;
    $(document).on('click', '.sujet', function () {
        $("#message_container").empty();
        current_id = $(this).parent("tr").data("value");
        $.ajax({
            url: '/Messagerie/ChangeTopLu',
            dataType: 'json',
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({id : current_id}),
            success: function (data) {
                
                $.ajax({
                    url: '/Messagerie/GetMessage',
                    dataType: 'json',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    data: JSON.stringify({id : current_id}),
                    success: function (data) {
                        $("#message_container").append(data.html);
                        $('#modal_message').modal('open');
                        load();
                    }
                });
            }
        });
    });
    $("#btn_repondre").click(function () {
        $('#modal_message').modal('close');
        $.ajax({
            url: '/Messagerie/GetMessageElements',
            dataType: 'json',
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({ id: current_id }),
            success: function (data) {
                $("#sujet_envoi").val('RE : ' + data.sujet);
                $("#texte_envoi").val(data.texte);
                $("#destinataire_envoi").val(data.expediteur);
                Materialize.updateTextFields();
                $('#modal_envoi').modal('open');
            }
        });
    });
    $("#add_destinataire").autoComplete({
        source: function (term, response) {
            xhr = $.getJSON('/Messagerie/AutocompleteLogin', { partial_name: term }, function (data) { response(data); });
        }
    });
    $("#btn_add").click(function () {
        if ($("#add_destinataire").val() != "") {
            $("#destinataire_envoi").val($("#destinataire_envoi").val() + $("#add_destinataire").val().split(':')[0].trim() + ";");
        }
        $("#liste_joueurs option:selected").each(function () {
            $("#destinataire_envoi").val($("#destinataire_envoi").val() + $(this).val() + ";");
        });
        Materialize.updateTextFields();
    });
    $("#form_envoi").validate({
        rules :{
            destinataire_envoi: {
                required : true
            }
        },
        messages: {
            destinataire_envoi: {
                required : "veuillez complèter ce champs"
            }
        },
        submitHandler: function () {
            Send();
            return false;
        }
    });
    function Send () {
        var logins = $("#destinataire_envoi").val().split(';');
        var sujet = $("#sujet_envoi").val();
        var texte = $("#message_envoi").val();
        $.ajax({
            url: '/Messagerie/SendMessage',
            dataType: 'json',
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({ logins : logins, sujet : sujet, texte : texte }),
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Message envoyé", 3000);
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
    };
    $("#send_modal").click(function () {
        $("#modal_envoi").modal('open');
    });
    $(document).on('click', '.supp', function () {
        current_id = $(this).parent("tr").data("value");
        $.ajax({
            url: '/Messagerie/DeleteMessage',
            dataType: 'json',
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({ id : current_id }),
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Message supprimé", 3000);
                    load();
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
    });
    $("#liste_categorie").change(function () {
        $("#liste_joueurs").empty();
        var categorie = $(this).val();
        $.ajax({
            url: '/Messagerie/LoadListJoueurParCategorie',
            dataType: 'json',
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({ categorie : categorie }),
            success: function (data) {
                if (data.ok) {
                    $("#liste_joueurs").append(data.html);
                    $("select").material_select();
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
    });
});