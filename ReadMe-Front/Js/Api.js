const baseUrl = "/api/cartapi";
const favoriteUrl = "/api/favoriteapi";
const searchUrl = "/api/searchapi";

window.Api = {

    // 取得商品資料
    async fetchProducts(productid) {
        const response = await fetch(`${baseUrl}/getProducts?id=${productId}`, {
            method: "GET",
        });
        return await response.json(); // 返回商品資料
    },
    // 取得作者資料
    async fetchAuthors(author) {
        const response = await fetch(`${baseUrl}/getAuthors?author=${author}`, {
            method: "GET",
        });
        return await response.json(); // 返回作者資料
    },
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

    // 新增商品到購物車
    async addCart(productId, price) {
        const response = await fetch(`${baseUrl}/addCart`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ productId, price }),
        });

        if (!response.ok) {
            throw new Error((await response.json()).message || "加入購物車失敗");
        }

        return await response.json();
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
    // API 調用 - 新增訂單
    async createOrder(cartId) {
        try {
            const response = await fetch(`${baseUrl}/CreateOrder?cartid=${cartId}`, {
                method: "GET",
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json(); // 假設返回新增訂單的結果
            console.log(`Order created for Cart ID: ${cartId}. Result:`, result);
            return result; // 返回訂單資訊
        } catch (error) {
            console.error("Failed to create order:", error);
            throw error; // 向外層拋出錯誤
        }
    },

    // API 調用 - 清空購物車
    async  clearCart(cartId) {
    try {
        const response = await fetch(`${baseUrl}/clearCart?cartid=${cartId}`, {
            method: "DELETE",
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const result = await response.text(); // 假設返回清空操作的結果
        console.log(`Cart with ID: ${cartId} has been cleared. Message: ${result}`);
        return result;
    } catch (error) {
        console.error("Failed to clear cart:", error);
        throw error;
    }
},

// API 調用 - 查詢訂單
    async fetchOrder(orderName) {
    try {
        const response = await fetch(`${baseUrl}/getOrder?orderName=${orderName}`, {
            method: "GET",
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const result = await response.json(); // 假設返回訂單詳細資訊
        console.log(`Order details for ${orderName}:`, result);
        return result; // 返回訂單資訊
    } catch (error) {
        console.error("Failed to fetch order:", error);
        throw error;
    }
    },

    // 取得會員訂單資料
    async fetchOrders() {
        const response = await fetch(`/api/memberapi/getMemberOrder`, {
            method: "GET",
        });
        return await response.json(); // 取得資料訂單資料

    },

    // 取得會員訂單詳細資料
    async fetchOrdersDetail(orderName) {
        const response = await fetch(`/api/memberapi/getMemberOrderDetail?orderName=${orderName}`, {
            method: "GET",
        });
        return await response.json(); // 取得資料訂單資料

    },


};
