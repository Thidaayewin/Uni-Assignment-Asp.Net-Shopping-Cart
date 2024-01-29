

function incrementPrice(carts, index) {
	var quantity = parseInt($('input[name="quantity[' + index + ']"]').val());
	quantity++;
	$('input[name="quantity[' + index + ']"]').val(quantity);
	updatePrice(carts,index);
}

function decrementPrice(carts, index) {
	var quantity = parseInt($('input[name="quantity[' + index + ']"]').val());
	if (quantity == 1) {
		var result = confirm("Confirm remove item from cart?");
		if (!result) {
			return;
		}
	}
	quantity--;
	$('input[name="quantity[' + index + ']"]').val(quantity);
	updatePrice(carts, index);
}


function removeRow(carts, index) {
	var result = confirm("Confirm remove items from cart?");
	if (!result) {
		return;
	}
	$('input[name="quantity[' + index + ']"]').val(0);
	updatePrice(carts, index); //removerow of each item in cart
}


function updatePrice(carts, index) {
	var quantity = parseInt($('input[name="quantity[' + index + ']"]').val());
	var price = carts;
	var total = quantity * price;
	$('#totalPrice_' + index).text(total.toFixed(2));
	var hiddenInputProductItemId = document.getElementById(index).value;
	var hiddenIputCustomerId = document.getElementById("hidden-customer-id").value;
	onLoadData(index, quantity, total, hiddenInputProductItemId, hiddenIputCustomerId);
}

function onLoadData(index, quantity, total, hiddenInputProductItemId, hiddenIputCustomerId) {

    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Cart/onLoadData");
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            let data = JSON.parse(this.responseText);
			if (data.status === 200) {
				$('#totalPrice').text(data.totalCost.toFixed(2));
				$('#totalSum').text(data.totalCost.toFixed(2));
				addCount();
				//reload the webpage
				if (quantity == 0) {
					location.reload();
				}
            }
        }
    };
	xhr.send("index=" + index + "&quantity=" + quantity + "&total=" + total
		+ "&product_item_id=" + hiddenInputProductItemId + "&customer_id=" + hiddenIputCustomerId);


}

function comfirmCheckOut() {
	let xhr = new XMLHttpRequest();
	xhr.open("POST", "/Cart/checkOutOrder");
	xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	xhr.onreadystatechange = function () {
		if (this.readyState == XMLHttpRequest.DONE) {
			let data = JSON.parse(this.responseText);
			if (data.status === 200) {
				swal("Successful!", "Your order is confirmed!", "success").then((value) => {
					window.location.href = "/Purchase/PurchaseHistory";
				});
				
			} else if (data.status == 404) {
				window.location.href = "/Account/Login";
			} else if (data.status == 400) {
				swal("No Check out!", "There is no checkout items in there!", "error");
			}
		}
	};
	xhr.send();
}
