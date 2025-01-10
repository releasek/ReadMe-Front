const baseUrl = "/api/cartapi/";

// 取得購物車資料
export async function fetchCart(account) {
    try {
        const response = await fetch(`${baseUrl}/${account}`);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error("Failed to fetch cart data:", error);
        throw error;
    }
}
// 刪除購物車項目
export async function deleteCartItem(id) {
    try {
        const response = await fetch(`${baseUrl}/${cartItemId}`, {
            method: "DELETE",
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.text();
    } catch (error) {
        console.error("Failed to delete cart item:", error);
        throw error;
    }
}
// 刪除整個購物車
export async function deleteCart(cartId) {
    try {
        const response = await fetch(`${baseUrl}/remove-cart?cartId=${cartId}`, {
            method: "DELETE",
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const result = await response.text();
        console.log(`Cart with ID: ${cartId} has been deleted. Message: ${result}`);
        return result;
    } catch (error) {
        console.error("Failed to delete cart:", error);
        throw error;
    }
}
