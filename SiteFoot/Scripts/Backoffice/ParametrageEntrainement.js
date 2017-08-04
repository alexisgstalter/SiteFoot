$(document).ready(function () {
    $("select").material_select();
    $("#heure_debut, #heure_fin").pickatime({
        twelvehour: false,
        autoclose: true,
        cleartext: 'Effacer', // text for clear-button
        canceltext: 'Annuler', // Text for cancel-button
    });

    $("#date_debut, #date_fin").pickadate({
        autoclose: true,
        format : "dd/mm/yyyy"
    });
    $("#calendrier_entrainement").fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay',
            lang: 'fr'
        },
        allDayDefault: false,
        startParam: 'start',
        endParam: 'end',
        eventSources: [
            {
                url: "/Calendrier/GetEventsEntrainement",
                type: "POST",
                data: function () {
                    return {
                        id_entraineur: 0,
                        id_equipe: 0,
                        id_membre: "",
                        id_terrain: 0
                    };
                },
                dataType: "json",
                success: function (data) {
                },
                error: function (xhr, status, error) {
                    alert(error + " " + status);
                }
            }
        ],
        timeFormat: 'H:mm',
        eventClick: function (calEvent, jsEvent, view) {
        },
        displayEventEnd: true
    });
    $("#form_creneau_entrainement").validate({
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent("div"));
        },
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            equipe: {
                required : true
            },
            heure_debut: {
                required : true
            },
            heure_fin: {
                required : true
            },
            jour_semaine: {
                required : true
            },
            date_debut: {
                required : true
            },
            date_fin: {
                required : true
            }
        },
        messages: {
            equipe: {
                required: "veuillez renseigner ce champs"
            },
            heure_debut: {
                required: "veuillez renseigner ce champs"
            },
            heure_fin: {
                required: "veuillez renseigner ce champs"
            },
            jour_semaine: {
                required: "veuillez renseigner ce champs"
            },
            date_debut: {
                required: "veuillez renseigner ce champs"
            },
            date_fin: {
                required: "veuillez renseigner ce champs"
            }
        },
        submitHandler: function () {
            Create();
            return false;
        }
    });
    function Create() {
        LoadingScreen();
        var intitule = $("#intitule").val();
        var equipe = [];

        $("#equipe option:selected").each(function () {
            equipe.push($(this).val());
        });

        var jour_semaine = [];
        $("#jour_semaine option:selected").each(function () {
            jour_semaine.push($(this).text());
        });

        var terrain = $("#terrain").val();
        var date_debut = $("#date_debut").val();
        var date_fin = $("#date_fin").val();
        var heure_debut = $("#heure_debut").val();
        var heure_fin = $("#heure_fin").val();
        $.ajax({
            type: "POST",
            url: "/Backoffice/SaveConfigEntrainement",
            data: JSON.stringify({ equipes: equipe, terrain: terrain, date_debut : date_debut, date_fin : date_fin, jour_semaine : jour_semaine, heure_debut : heure_debut, heure_fin : heure_fin, intitule: intitule}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                if (msg.ok) { //Il n'y a eu aucune erreur côté serveur

                        Materialize.toast('Les créneaux ont été ajoutés', 4000)
                    //$('#loginModal .modal-content').unblock();
                        $("#calendrier_entrainement").fullCalendar('refetchEvents');

                    
                }
                else {  //Il y eu une erreur côté serveur
                    alert(msg.error);
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
        EndLoadingScreen();
        return false;
    };
    $("#supp").click(function () {
        LoadingScreen();
        var equipe = [];

        $("#equipe option:selected").each(function () {
            equipe.push($(this).val());
        });

        var jour_semaine = [];
        $("#jour_semaine option:selected").each(function () {
            jour_semaine.push($(this).text());
        });

        var date_debut = $("#date_debut").val();
        var date_fin = $("#date_fin").val();
        $.ajax({
            type: "POST",
            url: "/Backoffice/DeleteConfigEntrainement",
            data: JSON.stringify({ equipes: equipe,  date_debut: date_debut, date_fin: date_fin, jour_semaine: jour_semaine }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                if (msg.ok) { //Il n'y a eu aucune erreur côté serveur
                    
                    Materialize.toast('Les créneaux ont été supprimés', 4000)
                    //$('#loginModal .modal-content').unblock();
                    $("#calendrier_entrainement").fullCalendar('refetchEvents');


                }
                else {  //Il y eu une erreur côté serveur
                    alert(msg.error);
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
        EndLoadingScreen();
    });
})