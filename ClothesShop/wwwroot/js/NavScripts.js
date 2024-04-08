// script.js

$(document).ready(function () {
    // ตรวจสอบตำแหน่งของหน้าจอเมื่อโหลดหน้า
    checkSticky();

    // ตรวจสอบตำแหน่งของหน้าจอเมื่อมีการเลื่อนหน้าจอ
    $(window).scroll(function () {
        checkSticky();
    });

    function checkSticky() {
        // ตรวจสอบว่าหน้าจออยู่ที่ตำแหน่งบนสุดหรือไม่
        var isAtTop = $(window).scrollTop() === 0;

        // ปรับเปลี่ยน HTML ตามเงื่อนไข
        if (!isAtTop) {
            $("#sticker-sticky-wrapper").addClass("is-sticky");
            $("#sticker").css({
                "width": "100vw",
                "position": "fixed",
                "top": "0px",
                "z-index": "inherit"
            });
        } else {
            $("#sticker-sticky-wrapper").removeClass("is-sticky");
            //$("#sticker").css();
            $("#sticker").css({
                "width": "",
                "position": "",
                "top": "",
                "z-index": ""
            });
        }
    }
});
