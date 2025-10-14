// aqui vamos a crear una variable//
let datatabla;
//Aqui vamos agregar las funciones de JS para la vista de Orden//
$(document).ready(function () {
    loadDatatable();
});
//aqui vamos a crear una funcion que es la que se va a encargar de la carga de todos los datos//
function loadDatatable() {
    datatabla = $("#tbData").DataTable({
        // Aqui vamos a cambiar el idiama //
        "language": {
            "lengthMenu": "Mostrar _MENU_ registros por pagina",
            "zeroRecords": "Nigun registros",
            "info": "Mostrando pagina _PAGE_ de _PAGES_",
            "infoEmpty": "No hay registros ",
            "infoFiltered": "(filtrado from _MAX_ registros totales)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero", 
                "last": "Ultimo", 
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        //Aqui es donde llamas el datatable con javaScripts//
        "ajax": {
            "url": "/Orden/ObtenerListaOrdenes"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "nombreCompleto", "width": "15%" },
            { "data": "telefono", "width": "15%" },
            { "data": "email", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                      <div class="text-center">
                      <a href="/Orden/Detalle/${data}" class="btn btn-success text-white" style="cursor: pointer;">
                      <i class="fas fa-edit"></i>
                      </a>
                      </div>
                    `;
                }, "width": "5%"
            }
        ]
    });
}