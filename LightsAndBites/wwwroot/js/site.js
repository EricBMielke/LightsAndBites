// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//Grab specific dataset for details

function GetSpecificData(Id) {
    $.ajax({
        dataType: 'json',
        success: function (data) {
            $("#titleInput").val(data.Title);
            $("#directorInput").val(data.DirectorName);
            $("#genreInput").val(data.Genre);
            $("#idInput").val(data.MovieId);
            document.documentElement.scrollTop = 0;
            return data;
        }
    })
}