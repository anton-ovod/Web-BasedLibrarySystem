// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function preventLinkClick(event) {
    event.preventDefault(); 
}

document.getElementById('newCover')
    .addEventListener('change', function (event)
    {
        const file = event.target.files[0];
        const preview = document.getElementById('coverPreview');

        if (file)
        {
            const reader = new FileReader();
            reader.onload = function (e) {
                preview.src = e.target.result;
            };
            reader.readAsDataURL(file);
        }
        else
        {
            preview.src = '/images/BookCoverPlaceholder.jpg';
        }
    });