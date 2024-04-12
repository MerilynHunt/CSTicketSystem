// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function deleteTicket(i) {
    $.ajax({
        url: 'Home/Delete',
        type: 'POST',
        data: {
            id: i
        },
        success: function () {
            window.location.reload();
        }
    });
}

function populateForm(i)
{
    $.ajax({
        url: 'Home/PopulateForm',
        type: 'GET',
        data: {
            id: i
        },
        dataType: 'json',
        success: function (response) {
            $("#Ticket_Name").val(response.name);
            $("#Ticket_Id").val(response.id);
            $("#Ticket_Description").val(response.description);
            //$("Ticket_CreationTime").val(response.creationtime);
            $("#Ticket_DueDate").val(response.dueDate);
            $("#form-button").val("Update Ticket");
            $("#form-action").attr("action", "/Home/Update");
        }
    });
}