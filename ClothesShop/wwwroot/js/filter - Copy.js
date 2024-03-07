//function filterProducts(products, filterOptions) {
//    // กรองรายการสินค้าตามตัวกรองที่กำหนด
//    return products.filter(function (product) {
//        // เช็คเงื่อนไขแต่ละอย่างในตัวกรองและเปรียบเทียบกับข้อมูลสินค้า
//        // ตัวอย่างเช่น ถ้าต้องการกรองตามประเภทสินค้า
//        if (filterOptions.category && product.PdtName !== filterOptions.category) {
//            return false;
//        }
//        // ให้เพิ่มเงื่อนไขการกรองอื่นๆ ตามต้องการ

//        // คืนค่า true เมื่อสินค้าผ่านการกรองทั้งหมด
//        return true;
//    });
//}
//// ใช้ jQuery AJAX เพื่อรับ JSON จาก URL
//$.ajax({
//    url: '/api/products',
//    dataType: 'json',
//    success: function (data) {
//        // เมื่อรับ JSON สำเร็จ
//        console.log(data); // แสดงข้อมูล JSON ใน console เพื่อตรวจสอบ
//        // ทำการ filter ข้อมูล JSON ตามต้องการ
//        var filteredProducts = filterProducts(data, { category: "เสื้อผ้า" }); // เช่น กรองเฉพาะสินค้าที่มีประเภทเสื้อผ้า
//        // นำข้อมูลที่ filter แล้วไปใช้งานต่อได้ตามต้องการ
//    },
//    error: function (xhr, status, error) {
//        // กรณีเกิดข้อผิดพลาดในการรับ JSON
//        console.error(status, error); // แสดงข้อผิดพลาดใน console เพื่อตรวจสอบ
//    }
//});

//// เมื่อมีการคลิกที่ checkbox
//$('input[type="checkbox"]').change(function () {
//    var selectedSizes = []; // สร้าง array เพื่อเก็บค่าขนาดที่ถูกเลือก
//    // วน loop ทุก checkbox เพื่อตรวจสอบค่าที่ถูกเลือกและเพิ่มเข้าไปใน array
//    $('input[type="checkbox"]:checked').each(function () {
//        var sizeId = $(this).attr('id'); // ดึง id ของ checkbox
//        selectedSizes.push(sizeId); // เพิ่ม id ของ checkbox ที่ถูกเลือกเข้าไปใน array
//    });

//    // เรียกฟังก์ชันเพื่อทำการกรองข้อมูล
//    filterProductsBySize(selectedSizes);
//});

//// ฟังก์ชันสำหรับกรองข้อมูลตามขนาดที่ถูกเลือก
//function filterProductsBySize(selectedSizes) {
//    // ทำการกรองข้อมูลตามขนาดที่ถูกเลือก
//    // โค้ดที่ใช้ในการกรองข้อมูล จะต้องเขียนเพิ่มเติมตามโครงสร้างข้อมูลและเงื่อนไขการกรองข้อมูลของคุณ
//    // เช่น เรียกใช้ API เพื่อรับข้อมูลใหม่หรือกรองข้อมูลที่อยู่ใน DOM แล้วแสดงเฉพาะข้อมูลที่ผ่านการกรองไว้
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