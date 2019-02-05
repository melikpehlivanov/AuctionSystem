$('.close-btn').on("click", removePicture);

$('#inputGroupFile01').on('change',
    function (e) {
        let files = e.target.files;
        let myId = $('#item-id').val();
        let title = $('#title').val();
        if (files.length > 0) {
            if (window.FormData !== undefined) {
                let data = new FormData();
                for (let x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }
                data.append('__RequestVerificationToken', $('input[name=__RequestVerificationToken]').val());
                $.ajax({
                    type: "POST",
                    url: `/Pictures/UploadPictures?id=${myId}&title=${title}`,
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (result) {
                        preview(result);
                    }
                });
            }
        }
    });

function preview(data) {
    $('[src="https://res.cloudinary.com/do72gylo3/image/upload/v1547833155/default-img.jpg"]').closest('li').remove();

    let gallery = $('#smallGallery');
    let count = gallery.children().length;

    for (let i = 0; i < data.urls.length; i++) {
        let url = data.urls[i];

        let li = $('<li>')
            .append($('<a>')
                .append($('<img>')
                    .attr("src", url)))
            .append($('<input>')
                .addClass('item-picture')
                .attr("value", url)
                .attr("hidden", "hidden")
                .attr("id", `urls[${i + count}]`)
                .attr("name", `urls[${i + count}]`))
            .append($('<a>')
                .addClass("close-btn")
                .text('x')
                .on("click", removePicture));

        gallery.append(li);
    }
}

function removePicture() {
    let closestLiElement = $(this).closest('li');
    let src = closestLiElement.children().attr('data-image');
    let args = src.substring(src.lastIndexOf('/') + 1);
    let tokens = args.split('/');
    let pictureId = tokens[0];

    closestLiElement.remove();

    if (window.FormData !== undefined) {
        var data = new FormData();
        data.append('__RequestVerificationToken', $('input[name=__RequestVerificationToken]').val());
        $.ajax({
            type: "POST",
            url: `/Pictures/DeletePictures?pictureId=${pictureId}`,
            contentType: false,
            processData: false,
            data: data
        });
    }

    let $inputs = $(".item-picture");
    $inputs.each(function (index) {
        $(this)
            .attr("id", `urls[${index}]`)
            .attr("name", `urls[${index}]`);
    });

    if ($inputs.length === 0) {
        let li = $('<li>')
            .append($('<a>')
                .append($('<img>')
                    .attr("src", "https://res.cloudinary.com/do72gylo3/image/upload/v1547833155/default-img.jpg")))
            .append($('<a>')
                .addClass("close-btn")
                .text('x')
                .on("click", removePicture));

        $('#smallGallery').append(li);
    }
}