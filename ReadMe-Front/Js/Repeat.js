const NavBar={
    template:`<nav class="navbar navbar-expand-sm fixed-top bg-light">
    <div class="container d-flex justify-content-between align-items-center" style="height: 50px;">
        <a class="navbar-brand" href="#">
            <img src="~/images/readme-high-resolution-logo-transparent.png" alt="Logo" class="img-fluid" style="height: 50px; object-fit: contain;">
        </a>
        <form class="d-flex ms-auto align-items-center">
            <a href="/Cart.html" class="ms-3">
                <i class="fa-solid fa-cart-plus mt-1"></i>
            </a>
            <a href="/Favorite.html" class="ms-3">
                <i class="fa-solid fa-heart mt-1"></i>
            </a>
            <a href="#" class="ms-3 btn btn-sm btn-outline-primary newline">登入</a>
            <a href="#" class="ms-2 btn btn-sm btn-outline-danger newline">註冊</a>
            <a href="#" class="ms-2 btn btn-sm btn-outline-secondary newline">
            <i class="fa-solid fa-user"></i> 個人
            </a>
        </form>        
    </div>
</nav>
`
};
// const Progress={
//     template:`<div class="container" style="margin-top : 70px; ">
//     <section class="progress-section">
//         <div class="steps">
//            <a href="/Cart.html" style=" text-decoration: none;"> <div class="step">1 購物車</div></a>
//            <a href="/OrderDetails.html" style=" text-decoration: none;"> <div class="step ">2 付款方式 </div></a>
//            <a href="#" style=" text-decoration: none;"> <div class="step ">3 完成</div></a>
//         </div>
//       </section>
// </div>`
// };

const Progress = {
    props: {
        currentPage: {
            type: Number,
            required: true,
        },
    },
    template: `
        <div class="container" style="margin-top: 70px;">
            <section class="progress-section">
                <div class="progress-bar">
                    <div class="progress-fill" :style="{ width: progressWidth }"></div>
                </div>
                <div class="steps">          
                        <div class="step" :class="{ 'active': currentPage >= 1 }">
                            <div class="step-circle">1</div>
                            <div class="step-label">購物車</div>
                        </div>
                    
                   
                        <div class="step" :class="{ 'active': currentPage >= 2 }">
                            <div class="step-circle">2</div>
                            <div class="step-label">付款方式</div>
                        </div>
                   
                   
                        <div class="step" :class="{ 'active': currentPage >= 3 }">
                            <div class="step-circle check">✓</div>
                            <div class="step-label">完成</div>
                        </div>
                   
                </div>
            </section>
        </div>
    `,
    computed: {
        progressWidth() {
            return `${((this.currentPage - 1) / 2) * 100}%`;
        },
    },
};





