
const searchInput = document.querySelector("[data-search]")

const productTemplate = document.querySelector('[data-product-template]')
const productContainer = document.querySelector("[data-user-products-container]")

const noResultsMessage = document.querySelector('[data-no-results-message]');


let products = [];


//search and display products based on search input
searchInput.addEventListener("input", e => {
    const value = e.target.value.toLowerCase()
    let hasResults = false;
    products.forEach(product => {
        const isVisible =
            product.name.toLowerCase().includes(value) ||
            product.description.toLowerCase().includes(value)
        if (isVisible) {
            hasResults = true;
        }
        product.element.classList.toggle("hide", !isVisible)
    })
    
    if (!hasResults) {
        noResultsMessage.style.display = 'block';
    } else {
        noResultsMessage.style.display = 'none';
    }

})


var myValue = JSON.parse(document.getElementById("my-div").getAttribute("data-value"));
console.log(myValue);



products = myValue.map(product => {
    const productElement = productTemplate.content.cloneNode(true).children[0];
    const image = productElement.querySelector('.card-img-top');
    const title = productElement.querySelector('.card-title');
    const ratingnumber = productElement.querySelector("[rating-number]");
    const ratingstars = productElement.querySelector('.stars-inner');
    const description = productElement.querySelector("[data-description]");
    const price = productElement.querySelector("[data-price]");
    const addToCartButton = productElement.querySelector('[onclick]');

    //calculate how many stars to light up
    const starPercentage = (product.Ratings / 5) * 100; //divide by 5 because using 5 stars rating system
    const starPercentageRounded = `${Math.round(starPercentage / 10) * 10}%`;

    image.src = product.ImageUrl;
    image.alt = product.Name;
    title.textContent = product.Name;
    ratingnumber.textContent = product.Ratings !== null ? product.Ratings.toFixed(1) : "No ratings yet";
    ratingstars.style.width = product.Ratings !== null ? starPercentageRounded: 0;
    description.textContent = product.Description;
    price.textContent = "$ " + product.Price.toFixed(2); 
    addToCartButton.setAttribute('onclick', 'addToCart(${product.Id})');
    addToCartButton.addEventListener('click', () => {
        addToCart(product.Id);
    });
    productContainer.append(productElement);
    return { name: product.Name, description: product.Description, element: productElement };

});

/*add to cart button*/
function addToCart(productId) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Product/AddToCart");
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            let data = JSON.parse(this.responseText);
            if (data.status === 200) {
                showMessage("success");
                fixCount(data.total_quantity);
            } 
        }
    };
    xhr.send("productId=" + productId);
}
function showMessage(message) {
    if (message == "success") {
        const successMessage = document.getElementById('successMessage');
        successMessage.style.display = 'block';
        setTimeout(() => {
            successMessage.style.display = 'none';
        }, 3000);
    } else {
        const errorMessage = document.getElementById('errorMessage');
        errorMessage.style.display = 'block';
        setTimeout(() => {
            errorMessage.style.display = 'none';
        }, 3000);
    }
    
}
