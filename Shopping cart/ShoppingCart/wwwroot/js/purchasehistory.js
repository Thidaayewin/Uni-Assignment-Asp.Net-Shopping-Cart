//check for existing rating
//display the stars and make it clickable

//Actual Code
const allstars = document.querySelectorAll('.stars');
allstars.forEach((stars) => {
	const children = stars.children;
	for (let i = 2; i < children.length; i++) {
		const star = children[i];
		star.onclick = function () {
			let currentstar = i + 1;
			for (let j = 2; j < children.length; j++) {
				if (currentstar >= j + 1) {
					children[j].innerHTML = '&#9733';
				} else {
					children[j].innerHTML = '&#9734';
				}
			}
			var container = $(this).closest('.star-rating-container');
			var customerid = container.find('#hidden-customerid').val();
			var productid = container.find('#hidden-productid').val();
			console.log('Customer ID:', customerid);
			console.log('Product ID:', productid);
			const rating = currentstar-2;
			submitreview(customerid, productid, rating);
		}
	}
});

//save the rating 
function submitreview(customerid,productid,rating) {
	
	let xhr = new XMLHttpRequest();
	xhr.open("POST", "/Purchase/SavingReview");
	xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	xhr.onreadystatechange = function () {
		if (this.readyState == XMLHttpRequest.DONE) {
			let data = JSON.parse(this.responseText);
			if (data.success === true) {
				console.log('Review saved.');

			} else {
				console.log(xhr.responseText);
			}
		}
	};
	xhr.send("customerid=" + customerid + "&productid=" + productid + "&rating=" + rating);
}
