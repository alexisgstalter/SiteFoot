$(document).ready(function () {
    
    switch (window.location.pathname) {
        case '/':
            $('#accueil_list').addClass('active');
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
            $('#educateur_list').addClass('active');
            $("#collapse_calendrier").collapsible('open', 0);
            break;
        case '/Home/About':
            $("#about_list").addClass('active');
            break;
        case '/Home/Annonces':
            $('#annonces_list').addClass('active');
            break;
    }
    $(".active").css("background-color", "#64b5f6");
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
        var rememberme = $("#remember_me").is(":checked");
        //$('#loginModal .modal-content').block({ baseZ: 2500, message: '<h1><img src="/Images/Site/busy.gif" /> </h1>' });
        $.ajax({
            type: "POST",
            url: "/Home/Login",
            data: '{username:"' + username + '",password:"' + password + '",rememberme:"' + rememberme + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                if (msg.ok) { //Il n'y a eu aucune erreur côté serveur
                    if (msg.isGranted) {    //L'utilisateur est connecté
                        //$('#loginModal .modal-content').unblock();
                        location.replace("/")
                        
                    }
                    else {  //L'utilisateur n'est pas connecté
                        Materialize.toast('Identifiants incorrects', 4000)
                        //$('#loginModal .modal-content').unblock();
                    }
                }
                else {  //Il y eu une erreur côté serveur
                    alert(msg.error);
                }
            }
        });
        /*}*/
    };
})

