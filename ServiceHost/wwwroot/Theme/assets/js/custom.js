const cookieName = "cart-items";

/* ---------------------- پیام افزودن به سبد ---------------------- */
function showCartMessage() {
    const box = document.getElementById("cartMessage");
    const progress = document.getElementById("cartMessageProgress");
    const closeBtn = document.getElementById("cartMessageClose");
    const duration = 5000; // 5 ثانیه
    let timerID;

    if (!box || !progress) {
        console.error("Cart message element not found in DOM!");
        return;
    }

    box.style.display = "block";
    box.style.opacity = "1";
    box.style.animation = "bounceDown 0.6s ease forwards";

    progress.style.transition = "none";
    progress.style.width = "0%";

    requestAnimationFrame(() => {
        progress.style.transition = `width ${duration}ms linear`;
        progress.style.width = "100%";
    });

    timerID = setTimeout(() => hideCartMessage(), duration);

    closeBtn.onclick = () => {
        clearTimeout(timerID);
        hideCartMessage();
    };

    function hideCartMessage() {
        box.style.animation = "fadeOut 0.5s ease forwards";
        setTimeout(() => {
            box.style.display = "none";
            progress.style.width = "0%";
        }, 500);
    }
}

/* ---------------------- افزودن محصول ---------------------- */
function addToCart(id, name, price, picture, Description) {
    const numericPrice = parseFloat(String(price).replace(/[^0-9.]/g, ""));
    let products = $.cookie(cookieName);
    products = products ? JSON.parse(products) : [];

    const count = parseFloat($("#productCount").val()); // ✅ مقدار اعشاری

    // ❌ هیچ بررسی برای تکرار نکن؛ هر بار یک آیتم جدید بساز
    products.push({
        id,
        name,
        unitPrice: numericPrice,
        picture,
        count: count,
        isInStock: true,
        discountRate: 0,
        discountAmount: 0,
        itemPayAmount: numericPrice,
        Description,
    });

    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();
    showCartMessage();
}

/* ---------------------- بروزرسانی سبد در منو ---------------------- */
function updateCart() {
    let products = $.cookie(cookieName);
    products = JSON.parse(products);
    $("#cart_items_count").text(products.length);
    const cartItemsWrapper = $("#cart_items_wrapper");
    cartItemsWrapper.html('');
    products.forEach(x => {
        const productHTML = `
            <div class="cart-item" style="padding:10px 0; border-bottom:1px solid #ddd;">
                <div class="shopping-cart-img" style="display:inline-block; vertical-align:top;">
                    <img src="/ProductPictures/${x.picture}" 
                         alt="${x.name}" 
                         style="width:60px; height:auto; border-radius:5px;">
                </div>
                <div class="shopping-cart-title" style="display:inline-block; margin-right:10px; vertical-align:top;">
                    <h6 style="margin:0;"><a href="shop-product-right.html">${x.name}</a></h6>
                    <h6 style="margin:0; font-size:14px; color:#666;">قیمت: ${x.unitPrice} تومان</h6>
                    <p style="margin:0; font-size:13px;">متراژ: ${x.count}</p>
                </div>
                <div class="shopping-cart-delete" style="display:inline-block; float:left;">
                    
                  <a href="javascript:void(0)"
   onclick="removeFromCart('${x.id}', '${x.count}')"
   style="color:red; cursor:pointer;">
   <i class="fi-rs-cross-small"></i>
</a>

                </a>
                </div>
            </div>
        `;
        cartItemsWrapper.append(productHTML);
    });
}


function removeFromCart(id, count) {
    let products = $.cookie(cookieName);
    products = JSON.parse(products);

    // حذف دقیق با تطبیق id و count
    const itemToRemove = products.findIndex(x => x.id === id && Math.abs(parseFloat(x.count) - parseFloat(count)) < 0.001);
    if (itemToRemove >= 0) {
        products.splice(itemToRemove, 1);
    }

    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();
    location.reload(); // صفحه Cart رو رفرش کن


    // 👇 حذف هم‌زمان از سرور
    fetch(`/Cart?handler=RemoveFromCart&id=${id}&count=${count}`, {
        method: "GET",
        credentials: "same-origin"
    }).then(() => {
        console.log("Removed from server-side cookie successfully");
    });
}







/* ---------------------- تغییر تعداد در صفحه سبد ---------------------- */
function changeCartItemCount(id, totalId, count) {
    let products = $.cookie(cookieName);
    products = JSON.parse(products);

    const productIndex = products.findIndex(x => x.id == id);
    const newCount = parseFloat(count);

    products[productIndex].count = newCount;

    const product = products[productIndex];
    const newPrice = parseFloat(product.unitPrice) * newCount;

    $(`#${totalId}`).text(newPrice.toLocaleString('fa-IR'));

    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();

    const settings = {
        url: "https://localhost:5001/api/inventory",
        method: "POST",
        timeout: 0,
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify({ productId: id, count: newCount })
    };

    $.ajax(settings).done(function (data) {
        const warningId = `warning-${id}`;
        const warningsDiv = $('#productStockWarnings');

        if (data.isStock === false) {
            if ($(`#${warningId}`).length == 0) {
                warningsDiv.append(`
                    <div class="alert alert-warning" id="${warningId}">
                        <i class="fa fa-warning"></i> کالای <strong>${data.productName}</strong>
                        کمتر از مقدار درخواستی در انبار موجود است.
                    </div>
                `);
            }
        } else {
            $(`#${warningId}`).remove();
        }
    }).fail(() => alert("خطایی رخ داده است. لطفا با مدیر سیستم تماس بگیرید."));
}
