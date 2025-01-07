const baseUrl = "/api/cartapi";
const favoriteUrl = "/api/favoriteapi";

window.Api = {
    // 取得購物車資料
    async fetchCart() {
        try {
            const response = await fetch(`${baseUrl}/getAllCartItems`, {
                method: "GET",
            });

            if (!response.ok) {
                if (response.status === 400) {
                    throw new Error("帳號為空，請提供有效帳號！");
                } else if (response.status === 404) {
                    throw new Error("找不到購物車資料！");
                } else {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
            }

            return await response.json(); // 返回購物車資料
        } catch (error) {
            console.error("Failed to fetch cart data:", error);
            throw error; // 向外層拋出錯誤
        }
    },

    // 取得折價券資料
    async fetchPromotions() {
        const response = await fetch(`${baseUrl}/getPromotions`, {
            method: "GET",
        });
        return await response.json(); // 返回折價券資料
    },

    // 取得收藏清單資料
    async fetchFavorite() {
        const response = await fetch(`${favoriteUrl}`, {
            method: "GET",
        });
        return await response.json(); // 返回折價券資料
    },

    // 刪除收藏清單
    async deleteFavorite(favoriteItemId) {
        try {
            const response = await fetch(`${favoriteUrl}/${favoriteItemId}`, {
                method: "DELETE",
            });

            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

            const result = await response.text();
            console.log(`Deleted favorite item ID: ${favoriteItemId}. Message: ${result}`);
            return result;
        } catch (error) {
            console.error("Error deleting favorite item:", error);
            throw error;
        }
    },

    // 新增商品到收藏清單
    async addToWishlist( productid) {
        const response = await fetch(`${baseUrl}/addToFavorite?productid=${productid}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ productid }),
        });

        if (!response.ok) {
            throw new Error(`加入收藏清單失敗：${await response.text()}`);
        }

        return await response.json(); // 返回結果
    },



    // 刪除購物車項目
    async deleteCartItem(cartItemId) {
        try {
            const response = await fetch(`${baseUrl}/deleteCartIrem?cartItemId=${cartItemId}`, {
                method: "DELETE",
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.text(); // 返回刪除操作的結果
            console.log(`Cart item with ID: ${cartItemId} has been deleted. Message: ${result}`);
            return result;
        } catch (error) {
            console.error("Failed to delete cart item:", error);
            throw error; // 向外部拋出錯誤
        }
    },

    // 刪除整個購物車
    async deleteCart(cartId) {
        try {
            const response = await fetch(`${baseUrl}/remove-cart?cartId=${cartId}`, {
                method: "DELETE",
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.text(); // 返回刪除操作的結果
            console.log(`Cart with ID: ${cartId} has been deleted. Message: ${result}`);
            return result;
        } catch (error) {
            console.error("Failed to delete cart:", error);
            throw error; // 向外層拋出錯誤
        }
    },
};
