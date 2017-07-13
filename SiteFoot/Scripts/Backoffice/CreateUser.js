$(document).ready(function () {
    Load();
    function Load() {
        $("#user_container").empty();
        $.ajax({
            url: "GetUtilisateurs",
            type: "POST",
            data: "{}",
            dataType: "json",
            success: function (data) {
                if (data.ok) {
                    $("#user_container").append(data.html);
                    $("table").dataTable({
                        "scrollY": false,
                        "scrollX": false,
                        "scrollCollapse": false,
                        "searching": true,
                        "ordering": true,
                        "info": true,
                        "paging": true,
                        "order": []
                    });
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
    }
    $("#add_modal").click(function () {
        $("#modal_add_user").modal('open');
    });

    $.validator.addMethod("mdp", function (value, element) {
        return value == $("#password").val();
    }, "Le mot de passe doit être identique");
    $("#form_create_user").validate({
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent("div"));
        },
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            login: {
                required : true
            },
            password: {
                required : true
            },
            confirm_password: {
                required: true,
                mdp : true
            },
            email: {
                email: true,
                required : true
            },
            telephone: {
                required : true
            },
            nom: {
                required : true
            },
            prenom: {
                required : true
            }
        },
        messages: {
            login: {
                required: "veuillez renseigner ce champs"
            },
            password: {
                required: "veuillez renseigner ce champs"
            },
            confirm_password: {
                required: "veuillez renseigner ce champs"
            },
            email: {
                email: "le format d'email est incorrect",
                required: "veuillez renseigner ce champs"
            },
            telephone: {
                required: "veuillez renseigner ce champs"
            },
            nom: {
                required: "veuillez renseigner ce champs"
            },
            prenom: {
                required: "veuillez renseigner ce champs"
            }
        },
        submitHandler: function (form) {
            Create();
            return false;
        }
    })
    function Create() {
        var login = $("#login").val();
        var password = $("#password").val();
        var groupe = [];

        $("#groupe option:selected").each(function () {
            groupe.push($(this).val());
        });

        var nom = $("#nom").val();
        var prenom = $("#prenom").val();
        var email = $("#email").val();
        var telephone = $("#telephone").val();
        $.ajax({
            url: "SaveUser",
            type: "POST",
            data: JSON.stringify({login : login, password : password, groupes : groupe, email : email, telephone : telephone, nom : nom, prenom : prenom}),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Utilisateur créé", 3000);
                    Load();
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
    $("#confirm_password").bind('input', function () {
        if ($(this).val() != $("#password").val()) {
            $(this).removeClass("valid");
            $(this).addClass("error");
        }
        else {
            $(this).removeClass("error");
            $(this).addClass("valid");
        }
    });

    var current_id;
    $(document).on('click', '.edit', function () {
        current_id = $(this).parent("tr").find("td").eq(0).text();
        var login = $(this).parent("tr").find("td").eq(1).text();
        var password = $(this).parent("tr").find("td").eq(2).text();
        var groupes = [];
        groupes = $(this).parent("tr").find("td").eq(3).text().split(',');
        var prenom = $(this).parent("tr").find("td").eq(4).text();
        var nom = $(this).parent("tr").find("td").eq(5).text();
        var email = $(this).parent("tr").find("td").eq(6).text();
        var telephone = $(this).parent("tr").find("td").eq(7).text();

        $("#login_edit").val(login);
        $("#password_edit").val(password);
        $("#confirm_password_edit").val(password);
        for (var j = 0; j < $("#groupe_edit option").length; j++) {

            $("#groupe_edit option").eq(j).prop("selected", "");
            
        }
        for (var i = 0; i < groupes.length; i++) {
            for (var j = 0; j < $("#groupe_edit option").length; j++) {
                if (groupes[i] == $("#groupe_edit option").eq(j).text()) {
                    $("#groupe_edit option").eq(j).prop("selected", "selected");
                }
            }
        }
        $("select").material_select();
        $("#prenom_edit").val(prenom);
        $("#nom_edit").val(nom);
        $("#telephone_edit").val(telephone);
        $("#email_edit").val(email);
        $("#modal_edit_user").modal('open');
        
    });
    $.validator.addMethod("mdp_edit", function (value, element) {
        return value == $("#password_edit").val();
    }, "Le mot de passe doit être identique");
    $("#form_modif_user").validate({
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent("div"));
        },
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            login_edit: {
                required: true
            },
            password_edit: {
                required: true
            },
            confirm_password_edit: {
                required: true,
                mdp_edit: true
            },
            email_edit: {
                email: true,
                required: true
            },
            telephone_edit: {
                required: true
            },
            nom_edit: {
                required: true
            },
            prenom_edit: {
                required: true
            }
        },
        messages: {
            login_edit: {
                required: "veuillez renseigner ce champs"
            },
            password_edit: {
                required: "veuillez renseigner ce champs"
            },
            confirm_password_edit: {
                required: "veuillez renseigner ce champs"
            },
            email_edit: {
                email: "le format d'email est incorrect",
                required: "veuillez renseigner ce champs"
            },
            telephone_edit: {
                required: "veuillez renseigner ce champs"
            },
            nom_edit: {
                required: "veuillez renseigner ce champs"
            },
            prenom_edit: {
                required: "veuillez renseigner ce champs"
            }
        },
        submitHandler: function (form) {
            Update();
            return false;
        }
    });
    function Update() {
        var login = $("#login_edit").val();
        var password = $("#password_edit").val();
        var groupe = [];

        $("#groupe_edit option:selected").each(function () {
            groupe.push($(this).val());
        });

        var nom = $("#nom_edit").val();
        var prenom = $("#prenom_edit").val();
        var email = $("#email_edit").val();
        var telephone = $("#telephone_edit").val();
        $.ajax({
            url: "UpdateUser",
            type: "POST",
            data: JSON.stringify({id: current_id, login: login, password: password, groupes: groupe, email: email, telephone: telephone, nom: nom, prenom: prenom }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Utilisateur Modifié", 3000);
                    Load();
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

    $(document).on("click", ".supp", function () {
        current_id = $(this).parent("tr").find("td").eq(0).text();
        $.ajax({
            url: "DeleteUser",
            type: "POST",
            data: JSON.stringify({ id: current_id}),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Utilisateur supprimé", 3000);
                    Load();
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
})