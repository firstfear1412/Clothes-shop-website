console.log("connect Script Success");
$('input[type = "checkbox"]').on('change', function () {
	var selectSizes = $('input[type = "checkbox"][name = "sizes"]:checked').map(function () {
		return $(this).val();
	}).get();
	var selectTypes = $('input[name="types"]:checked').map(function () {
		return $(this).val();
	}).get();
	var selectColors = $('input[name="colors"]:checked').map(function () {
		return $(this).val();
	}).get();
	var targetName = (document.getElementById('TargetProduct') ?? {}).value || "";
	console.log(selectSizes);
	$.ajax({
		//url: '@Url.Action("GetFilteredProducts", "Product")',
		url: '/Product/GetFilteredProducts',
		type: 'POST',
		dataType: 'html',
		data: {
			typeIds: selectTypes,
			sizeIds: selectSizes,
			colorIds: selectColors,
			targetName: targetName
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
//----------------------------------------------------------------------
function updateStock(PrefixId) {
	var selectedColor = document.querySelector('input[name="radioColors"]:checked').value;
	var selectedSize = document.querySelector('input[name="radioSizes"]:checked').value;
	SetProductId(PrefixId, selectedColor, selectedSize);
	// �觢�������ѧ������������͢͢����Ũӹǹ�Թ��Ҥ������
	$.ajax({
		url: '/Product/GetStock',
		type: 'POST',
		data: { PrefixPdId: PrefixId, colorId: selectedColor, sizeId: selectedSize },
		success: function (data) {
			// �Ѵ��áѺ�����ŷ�����Ѻ �� �ʴ�������������
			console.log(data); // ������ҧ����ʴ��Ţ����ŷ�����Ѻ
			if (parseInt(data) > 0) {
				// ������Թ��� ����Դ��ҹ Radio button �ͧ���ء���
				var stockQuantityElement = document.getElementById('stock-quantity');
				stockQuantityElement.innerText = data;
				var max = document.getElementById('quantity').max = data
				var cur = document.getElementById('quantity').value;
				if (parseInt(cur) > parseInt(max)) {
					document.getElementById('quantity').value = document.getElementById('quantity').max;
				}
				document.getElementById('quantity').disabled = false;
				document.getElementById('SendTocart').disabled = false;
			} else {
				// ���������Թ��� ���Դ��ҹ Radio button �ͧ���ء���¡�����������Թ���
				var stockQuantityElement = document.getElementById('stock-quantity');
				stockQuantityElement.innerText = 0;
				var cur = document.getElementById('quantity').value;
				cur = 0;
				document.getElementById('quantity').disabled = true;
				document.getElementById('SendTocart').disabled = true;
			}
		},
		error: function () {
			console.log("Error: Unable to fetch stock data.");
		}
	});
}
function SetProductId(PrefixId, selectedColor, selectedSize) {
	var ProductId = document.getElementById("productId").value = PrefixId + "-0" + selectedColor + "-0" + selectedSize;
	console.log("ProductId : " + ProductId);
}
