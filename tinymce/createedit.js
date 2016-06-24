function createEditor(selectorID)
{
    tinymce.init({
        selector: selectorID,
        theme: "modern",
        height:300,
        width:'100%',
        inline_styles: false,
        //paste_as_text: true,
        plugins: [
            "advlist autolink lists link image charmap print preview hr anchor pagebreak",
            "searchreplace wordcount visualblocks visualchars code fullscreen",
            "insertdatetime media nonbreaking save table contextmenu directionality",
            "emoticons template paste textcolor"
        ],
        toolbar1: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview | forecolor backcolor",
        toolbar_items_size: 'small',
        image_advtab: true,
    });
}