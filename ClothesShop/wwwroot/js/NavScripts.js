// script.js

$(document).ready(function () {
    // ��Ǩ�ͺ���˹觢ͧ˹�Ҩ��������Ŵ˹��
    checkSticky();

    // ��Ǩ�ͺ���˹觢ͧ˹�Ҩ�������ա������͹˹�Ҩ�
    $(window).scroll(function () {
        checkSticky();
    });

    function checkSticky() {
        // ��Ǩ�ͺ���˹�Ҩ���������˹觺��ش�������
        var isAtTop = $(window).scrollTop() === 0;

        // ��Ѻ����¹ HTML ������͹�
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
