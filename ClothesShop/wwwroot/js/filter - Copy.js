//function filterProducts(products, filterOptions) {
//    // ��ͧ��¡���Թ��ҵ����ǡ�ͧ����˹�
//    return products.filter(function (product) {
//        // �����͹��������ҧ㹵�ǡ�ͧ������º��º�Ѻ�������Թ���
//        // ������ҧ�� ��ҵ�ͧ��á�ͧ����������Թ���
//        if (filterOptions.category && product.PdtName !== filterOptions.category) {
//            return false;
//        }
//        // ����������͹䢡�á�ͧ���� �����ͧ���

//        // �׹��� true ������Թ��Ҽ�ҹ��á�ͧ������
//        return true;
//    });
//}
//// �� jQuery AJAX �����Ѻ JSON �ҡ URL
//$.ajax({
//    url: '/api/products',
//    dataType: 'json',
//    success: function (data) {
//        // ������Ѻ JSON �����
//        console.log(data); // �ʴ������� JSON � console ���͵�Ǩ�ͺ
//        // �ӡ�� filter ������ JSON �����ͧ���
//        var filteredProducts = filterProducts(data, { category: "����ͼ��" }); // �� ��ͧ੾���Թ��ҷ���ջ���������ͼ��
//        // �Ӣ����ŷ�� filter �������ҹ���������ͧ���
//    },
//    error: function (xhr, status, error) {
//        // �ó��Դ��ͼԴ��Ҵ㹡���Ѻ JSON
//        console.error(status, error); // �ʴ���ͼԴ��Ҵ� console ���͵�Ǩ�ͺ
//    }
//});

//// ������ա�ä�ԡ��� checkbox
//$('input[type="checkbox"]').change(function () {
//    var selectedSizes = []; // ���ҧ array �����纤�Ң�Ҵ���١���͡
//    // ǹ loop �ء checkbox ���͵�Ǩ�ͺ��ҷ��١���͡������������ array
//    $('input[type="checkbox"]:checked').each(function () {
//        var sizeId = $(this).attr('id'); // �֧ id �ͧ checkbox
//        selectedSizes.push(sizeId); // ���� id �ͧ checkbox ���١���͡����� array
//    });

//    // ���¡�ѧ��ѹ���ͷӡ�á�ͧ������
//    filterProductsBySize(selectedSizes);
//});

//// �ѧ��ѹ����Ѻ��ͧ�����ŵ����Ҵ���١���͡
//function filterProductsBySize(selectedSizes) {
//    // �ӡ�á�ͧ�����ŵ����Ҵ���١���͡
//    // �鴷����㹡�á�ͧ������ �е�ͧ��¹�����������ç���ҧ������������͹䢡�á�ͧ�����Ţͧ�س
//    // �� ���¡�� API �����Ѻ�������������͡�ͧ�����ŷ������� DOM �����ʴ�੾�Т����ŷ���ҹ��á�ͧ���
//}

console.log("connect Script Success");
$('input[type = "checkbox"][name = "sizes"]').on('change', function () {
	var selectSizes = $('input[type = "checkbox"][name = "sizes"]:checked').map(function () {
		return $(this).val();
	}).get();
	console.log(selectSizes);
	$.ajax({
		//url: '@Url.Action("GetFilteredProducts", "Product")',
		url: '/Product/GetFilteredProducts',
		type: 'POST',
		dataType: 'html',
		data: {
			sizeIds: selectSizes
		},
		success: function (result) {
			//$('#product-list').html(result);
			$('#CardProduct-Filter').html(result);
		},
		error: function () {
			console.log('An error while get filtered');
		}
	});
});