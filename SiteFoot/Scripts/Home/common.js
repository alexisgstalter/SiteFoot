$(document).ready(function () {
    $('select').material_select();

    $(".table").addClass("striped");

    switch (window.location.pathname) {
        case '/':
            $('#annonces_list').addClass('active');
            $("#collapse_annonce").collapsible('open', 0);
            break;
        case '/Annonce/GestionAnnonce':
            $('#annonces_gerer').addClass('active');
            $("#collapse_annonce").collapsible('open', 0);
            break;
        case '/Calendrier/Buvette':
            $('#buvette_list').addClass('active');
            $("#collapse_calendrier").collapsible('open', 0);
            break;
        case '/Calendrier/Entrainement':
            $('#entrainement_list').addClass('active');
            $("#collapse_calendrier").collapsible('open', 0);
            break;
        case '/Calendrier/FormationEducateur':
            $('#formation_list').addClass('active');
            $("#collapse_calendrier").collapsible('open', 0);
            break;
        case '/Home/Annonces':
            $('#annonces_list').addClass('active');
            break;
        case '/Resultats/Championnat':
            $('#championnats_list').addClass('active');
            $("#collapse_resultats").collapsible('open', 0);
            break;
        case '/Resultats/Match':
            $('#matchs_list').addClass('active');
            $("#collapse_resultats").collapsible('open', 0);
            break;
        case '/Messagerie/BoiteDeReception':
            $('#boite_recep').addClass('active');
            $("#collapse_messagerie").collapsible('open', 0);
            break;
        case '/Coordonnees/CoordonneesEntraineurs':
            $('#coordonnees_entraineurs').addClass('active');
            $("#collapse_coordonnees").collapsible('open', 0);
            break;
        case '/Coordonnees/CoordonneesJoueurs':
            $('#coordonnees_joueurs').addClass('active');
            $("#collapse_coordonnees").collapsible('open', 0);
            break;
        case '/Backoffice/CreateUser':
            $('#creation_equipe_list').addClass('active');
            $("#collapse_admin").collapsible('open', 0);
            break;
        case '/Backoffice/GestionDesJoueurs':
            $('#gestiondesjoueurs').addClass('active');
            $("#collapse_admin").collapsible('open', 0);
            break;
        case '/Backoffice/CreateEquipe':
            $('#creation_equipe_list').addClass('active');
            $("#collapse_admin").collapsible('open', 0);
            break;
        case '/Backoffice/ParametrageEntrainement':
            $('#parametrageentrainement').addClass('active');
            $("#collapse_admin").collapsible('open', 0);
            break;
        case '/Backoffice/GestionDesGroupes':
            $('#gestiondesgroupes').addClass('active');
            $("#collapse_admin").collapsible('open', 0);
            break;
    }
    $('.modal').modal();
    $(".button-collapse").sideNav({
        closeOnClick: true
    });
    $("#form_connect").submit(function () {
        Login();
        return false;
    });
    $("#btn_fermer").click(function () {
        $(".modal").modal("close");
    })
    function Login() {

        /*if(formLogin.isFormValidated())
        {*/
        var username = $("#_username").val();
        var password = $("#_password").val();
        //$('#loginModal .modal-content').block({ baseZ: 2500, message: '<h1><img src="/Images/Site/busy.gif" /> </h1>' });
        $.ajax({
            type: "POST",
            url: "/Home/Login",
            data: '{username:"' + username + '",password:"' + password + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                if (msg.ok) { //Il n'y a eu aucune erreur côté serveur
                    if (msg.isGranted) {    //L'utilisateur est connecté
                        //$('#loginModal .modal-content').unblock();
                        location.replace("/");

                    }
                    else {  //L'utilisateur n'est pas connecté
                        Materialize.toast('Identifiants incorrects', 4000)
                        //$('#loginModal .modal-content').unblock();
                    }
                }
                else {  //Il y eu une erreur côté serveur
                    alert(msg.error);
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
        /*}*/
    };
});
function LoadingScreen() {
    $.blockUI.defaults.css = {
        padding: 0,
        margin: 0,
        width: '30%',
        top: '40%',
        left: '35%',
        textAlign: 'center',
        cursor: 'wait'
    };
    $.blockUI.defaults.baseZ = 5000;
    $.blockUI({ message: '<div class="progress"><div class="indeterminate" style="width: 70%"></div></div>' });
}

function EndLoadingScreen() {
    $.unblockUI();
}

