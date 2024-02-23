
$(function () {
    let input = document.getElementById("autocompleteTextBox");

    $(input).autocomplete({
        source: function (request, response) {
            response([]); // Clear previous suggestions

            $.ajax({
                url: "/api/products",
                dataType: "json",
                data: {
                    term: request.term
                },
                success: function (data) {
                    let labelsSet = new Set(); // Use a Set to store unique labels

                    // Filter and match input value with pdName, pdtName, or brandName
                    let matchedNames = data.filter(function (item) {
                        let searchTerm = request.term.toLowerCase();
                        let matchesPdName = item.pdName.toLowerCase().includes(searchTerm);
                        let matchesPdtName = item.pdtName.toLowerCase().includes(searchTerm);
                        let matchestargetName = item.targetName.toLowerCase().includes(searchTerm);

                        //[
                        //    {
                        //        "pdId": "P001-01-01",
                        //        "colorName": "ขาว",
                        //        "sizeName": "XS   ",
                        //        "pdName": "เสื้อยืด",
                        //        "pdPrice": 490,
                        //        "pdCost": 100,
                        //        "pdStk": 5,
                        //        "pdtName": "ส่วนบน",
                        //        "statusId": null,
                        //        "statusName": null,
                        //        "targetId": null,
                        //        "targetName": "ผู้ชาย",
                        //        "supId": null
                        //    },
                        //    {
                        //    "pdId": "P001-02-01",
                        //        "colorName": "ดำ",
                        //        "sizeName": "XS   ",
                        //        "pdName": "เสื้อยืด",
                        //        "pdPrice": 490,
                        //        "pdCost": 100,
                        //        "pdStk": 8,
                        //        "pdtName": "ส่วนบน",
                        //        "statusId": null,
                        //        "statusName": null,
                        //        "targetId": null,
                        //        "targetName": "ผู้ชาย",
                        //        "supId": null
                        //    }
                        //]
                      

                        // Determine which label to add to ensure uniqueness
                        if (matchesPdName) {
                            labelsSet.add(item.pdName);
                        }
                        if (matchesPdtName) {
                            labelsSet.add(item.pdtName);
                        }
                 
                        if (matchestargetName) {
                            labelsSet.add(item.targetName);
                        }


                        return matchesPdName || matchesPdtName || matchestargetName;
                    });

                    // Convert the Set of unique labels into the format expected by jQuery UI Autocomplete
                    let autocompleteSuggestions = Array.from(labelsSet).map(function (label) {
                        // Here, we use the label for both the value and label properties, but you may adjust as needed
                        return { label: label, value: label };
                        /*return { label: label.pdId, value: label };*/
                        /* item.pdId*/

                    });

                    // Return the filtered and formatted suggestions
                    response(autocompleteSuggestions);
                }
            });
        },
        minLength: 2 // Minimum length before triggering autocomplete
    });
});


//$(function () {

//        ptxt = $('#autocompleteTextBox').val();
//        $("#autocompleteTextBox").autocomplete({
//            source: function (request, response) {
//                $.ajax({


//                    url: "/api/products",
//                    dataType: "json",

//                    success: function (data) {
//                        var products = data.map(function (item) {
//                            return {
//                                label: `${item.pdId} - ${item.pdName} - ${item.brandName}` ,
//                                value: item.pdName
//                                /*    `${item.pdName} - ${item.pdtName} - ${item.brandName}`*/
//                                /*value: item.pdId */// หรือข้อมูลอื่น ๆ ที่คุณต้องการส่งกลับเมื่อผู้ใช้เลือกรายการ
//                            };
//                        });
//                        response(products);
//                    }
//                    //success: function (data) {
//                    //    response($.map(data.d, function (item) {
//                    //        return {
//                    //            label: item.pdName,
//                    //            val: item.pdName
//                    //        }
//                    //    }));
//                    //},


//                });
//            },
//            minLength: 1,
//        });  // จำนวนตัวอักษรขั้นต่ำที่จะเริ่มแสดงรายการ autocomplete
// });

//$(function () {
//    let input = document.getElementById("autocompleteTextBox");

//    $(input).autocomplete({
//        source: function (request, response) {
//            // Execute function on keyup
//            // Remove all previous suggestions
//            response([]);

//            // Perform ajax request to get data
//            $.ajax({
//                url: "/api/products",
//                dataType: "json",
//                data: {
//                    term: request.term
//                },
//                success: function (data) {
//                    // Filter and match input value with pdName, pdtName, or brandName
//                    let matchedNames = data.filter(function (item) {
//                        return item.pdName.toLowerCase().startsWith(request.term.toLowerCase()) ||
//                            item.pdtName.toLowerCase().startsWith(request.term.toLowerCase()) ||
//                            item.brandName.toLowerCase().startsWith(request.term.toLowerCase());
//                    });

//                    // Map the matched names to Autocomplete format
//                    let autocompleteSuggestions = matchedNames.map(function (item) {
//                        return {
//                            label: `${item.pdtName}`,
//                          /*  label: `${item.pdId} -${item.pdName} - ${item.pdtName} - ${item.brandName}`,*/
//                            value: item.pdId
//                        };
//                    });

//                    // Return the filtered and formatted suggestions
//                    response(autocompleteSuggestions);
//                }
//            });
//        },
//        minLength: 1 // Minimum length before triggering autocomplete
//    });
//});