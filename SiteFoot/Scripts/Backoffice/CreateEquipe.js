$(document).ready(function () {
   
    loadAllEquipe();



    function loadAllEquipe() {
        $('#requested_equipe').empty();
        $.ajax({
            type: "POST",
            url: "/Backoffice/LoadAllEquipe",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                if (msg.ok) { //Il n'y a eu aucune erreur côté serveur

                    $('#requested_equipe').append(msg.html);

                    $('#equipetable').DataTable({
                        "scrollY": false,
                        "scrollX": false,
                        "scrollCollapse": false,
                        "searching": true,
                        "ordering": true,
                        "info": true,
                        "paging": true,
                        "order": []
                    });
                    $.each($.fn.dataTable.fnTables(true), function (idx, singleTable) {
                        $(singleTable).dataTable().fnAdjustColumnSizing();
                    });
                }
                else {  //Il y eu une erreur côté serveur
                    alert(msg.error);
                    //On enleve le potentiel chargement de la page
                }
            }
        });
    }

})
