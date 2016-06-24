var ImageName = "";
$(function ()
{
    $.ajaxSetup({ async: false });
    ImageName = $('#ImageName').val();
    var dropbox = $('#dropbox'),
		message = $('.message', dropbox);

    dropbox.filedrop({

        paramname: 'pic',
        maxfiles: 5,
        maxfilesize: 100,
        url: '/DragDrop/Upload.aspx?ImageName=' + ImageName,
        uploadFinished: function (i, file, response)
        {
            $.data(file).addClass('done');
        },
        error: function (err, file)
        {
            switch (err)
            {
                case 'BrowserNotSupported':
                    showMessage('Your browser does not support HTML5 file uploads!');
                    break;
                case 'TooManyFiles':
                    alert('Too many files! Please select 5 at most! (configurable)');
                    break;
                case 'FileTooLarge':
                    alert(file.name + ' is too large! Please upload files up to 2mb (configurable).');
                    break;
                default:
                    break;
            }
        },
        //Called before each upload is started
        beforeEach: function (file)
        {
            if (file.type.match(/^image\//))
            {
                //alert(savePro());                
            }
            if (!file.type.match(/^image\//))
            {
                alert('Only images are allowed!');
                return false;
            }
        },

        uploadStarted: function (i, file, len)
        {
            createImage(file);
        },

        progressUpdated: function (i, file, progress)
        {
            $.data(file).find('.progress').width(progress);
        }

    });

    var template = '<div class="preview">' +
                        '<span class="imageHolder">' +
                            '<img />' +
                            '<span class="uploaded"></span>' +
                        '</span>' +
                        '<div class="progressHolder">' +
                            '<div class="progress"></div>' +
                        '</div>' +
                    '</div>';


    function createImage(file)
    {
        var preview = $(template),
            image = $('img', preview);

        var reader = new FileReader();

        image.width = 100;
        image.height = 100;

        reader.onload = function (e)
        {

            // e.target.result holds the DataURL which
            // can be used as a source of the image:
            //alert(e.target.result);
            image.attr('src', e.target.result);
        };

        // Reading the file as a DataURL. When finished,
        // this will trigger the onload function above:
        reader.readAsDataURL(file);

        message.hide();
        preview.appendTo(dropbox);

        // Associating a preview container
        // with the file, using jQuery's $.data():

        $.data(file, preview);
    }

    function showMessage(msg)
    {
        message.html(msg);
    }
});
function savePro()
{  //P_Name = 0,P_BrandID = 1,_Price = 2,P_MaxLength = 3,P_MaxWidth = 4;P_MaxHeight = 5;P_Diameter = 6; P_MfrItemCode = 7, P_ProductURL = 8, P_BrochureURL = 9,P_MfrItemType = 10,P_MfrItemCode2 = 11,P_ImageURL = 12;
    var str = "LG-SYS^7^^^^^^^^^^^~";
    console.log(ItemID + "before");
    $.post("../Data.aspx?Action=UpdateProducts&Values=" + str, function (data)
    {
        console.log(data);
        $('#itemID').html(data);
        ItemID = data;
    });
    console.log(ItemID + "after:");
    return ItemID;
}