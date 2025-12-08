// =============================================
// CONFIGURACIÓN GLOBAL - VERSIÓN DINÁMICA
// =============================================

const BACKEND_URL = window.location.origin;

const API_PRODUCTOS = `${BACKEND_URL}/Productos/ProductosPublicados`;
const API_CATEGORIAS = `${BACKEND_URL}/api/Categorias`;

const BACKEND_CONFIG = {
    URL: BACKEND_URL,
    ENDPOINTS: {
        ESTADOS_PEDIDOS: '/api/EstadoPedidos',
        INICIALIZAR_ESTADOS: '/api/EstadoPedidos/Inicializar',
        PEDIDOS: '/api/Pedidos'
    },
    WHATSAPP: '5493534014935'
};
// Estructuras completamente dinámicas
let products = {}; // Se llenará con categorías reales del backend
let categories = [];
let cart = [];
let currentProduct = null;
let currentCategory = null;
let editingItemIndex = -1;

// Carousel variables
let currentSlide = 0;
const totalSlides = 3;
let autoSlideInterval;

// =============================================
// ELEMENTOS DOM
// =============================================

const categoriesSection = document.getElementById('categoriesSection');
const productsSection = document.getElementById('productsSection');
const productDetailSection = document.getElementById('productDetailSection');
const categoryTitle = document.getElementById('categoryTitle');
const productsGrid = document.getElementById('productsGrid');
const backBtn = document.getElementById('backBtn');
const backToProductsBtn = document.getElementById('backToProductsBtn');
const cartBtn = document.getElementById('cartBtn');
const cartCount = document.getElementById('cartCount');
const cartModal = document.getElementById('cartModal');
const productModal = document.getElementById('productModal');
const checkoutModal = document.getElementById('checkoutModal');
const editCartModal = document.getElementById('editCartModal');

// =============================================
// INICIALIZACIÓN
// =============================================

document.addEventListener('DOMContentLoaded', function() {
    console.log('🚀 Inicializando aplicación...');
    initializeApp();
});
function initializeApp() {
    // Mobile menu
    document.getElementById('mobileMenuBtn').addEventListener('click', toggleMobileMenu);
    document.getElementById('mobileProductsBtn').addEventListener('click', toggleMobileProducts);

    // Auto-play carousel
    setInterval(nextSlide, 5000);

    // Smooth scrolling
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({ behavior: 'smooth' });
            }
            hideMobileMenu();
        });
    });

    // Botones y modales
    backBtn.addEventListener('click', showCategories);
    backToProductsBtn.addEventListener('click', () => showProducts(currentCategory));
    cartBtn.addEventListener('click', showCart);
    document.getElementById('closeCartBtn').addEventListener('click', hideCart);
    document.getElementById('closeProductBtn').addEventListener('click', hideProductModal);
    document.getElementById('closeCheckoutBtn').addEventListener('click', hideCheckoutModal);
    document.getElementById('closeEditBtn').addEventListener('click', hideEditModal);
    document.getElementById('checkoutBtn').addEventListener('click', showCheckout);
    document.getElementById('addToCartBtn').addEventListener('click', addToCart);
    document.getElementById('detailAddToCartBtn').addEventListener('click', addToCartFromDetail);
    document.getElementById('checkoutForm').addEventListener('submit', processOrder);
    document.getElementById('saveChangesBtn').addEventListener('click', saveCartChanges);
    document.getElementById('viewDetailsBtn').addEventListener('click', viewProductDetails);
    document.getElementById('editDecreaseBtn').addEventListener('click', () => changeEditQuantity(-1));
    document.getElementById('editIncreaseBtn').addEventListener('click', () => changeEditQuantity(1));

    // Cargar datos del backend
    loadCategories();
    loadProducts();
}

// =============================================
// FUNCIONES DE CARGA DESDE BACKEND
// =============================================

async function loadCategories() {
    try {
        console.log('🔄 Cargando categorías desde:', API_CATEGORIAS);
        const response = await fetch(API_CATEGORIAS);
        
        if (!response.ok) throw new Error(`Error ${response.status}: ${response.statusText}`);
        
        const categoriasData = await response.json();
        console.log('✅ Categorías cargadas del backend:', categoriasData.map(c => c.nombre));
        
        categories = categoriasData;
        renderCategories();
        updateNavigationMenu();
        
        // DEBUG: Comparar con categorías de productos
        setTimeout(() => {
            const categoriasEnProductos = Object.keys(products);
            console.log('🔍 COMPARACIÓN - Categorías en productos:', categoriasEnProductos);
            console.log('🔍 COMPARACIÓN - Categorías del backend:', categories.map(c => c.nombre));
            
            // Verificar coincidencias
            const coincidencias = categories.filter(cat => 
                categoriasEnProductos.includes(cat.nombre)
            );
            console.log('🔍 COMPARACIÓN - Coincidencias encontradas:', coincidencias.map(c => c.nombre));
        }, 1000);
        
    } catch (error) {
        console.error('❌ Error cargando categorías:', error);
        categories = getFallbackCategories();
        renderCategories();
        updateNavigationMenu();
    }
}
async function loadProducts() {
    try {
        console.log('🔄 Cargando productos PUBLICADOS desde:', API_PRODUCTOS);
        const response = await fetch(API_PRODUCTOS);
        
        if (!response.ok) throw new Error(`Error ${response.status}: ${response.statusText}`);
        
        const productosData = await response.json();
        console.log('✅ Productos PUBLICADOS cargados:', productosData);
        
        // Transformar los productos publicados al formato del frontend
        products = transformProductsToFrontendFormat(productosData);
        
        console.log('📊 Estadísticas de productos publicados:');
        Object.keys(products).forEach(categoria => {
            console.log(`   📦 ${categoria}: ${products[categoria].length} productos`);
        });
        
        // Si estamos en una vista de productos, actualizar
        if (currentCategory && productsSection && !productsSection.classList.contains('hidden')) {
            showProducts(currentCategory);
        }
        
    } catch (error) {
        console.error('❌ Error cargando productos publicados:', error);
        products = getFallbackProducts();
        mostrarNotificacion('Atención', 'No se pudieron cargar los productos publicados. Usando datos de ejemplo.', 'advertencia');
    }
}

// =============================================
// DIAGNÓSTICO COMPLETO DEL DTO RECIBIDO
// =============================================

function diagnosticoCompletoDTO() {
    console.log('=== 🔬 DIAGNÓSTICO COMPLETO DTO ===');
    
    if (products && Object.keys(products).length > 0) {
        const primeraCategoria = Object.keys(products)[0];
        const primerProducto = products[primeraCategoria][0];
        
        console.log('📦 PRODUCTO:', primerProducto.name);
        console.log('🔍 DATOS ORIGINALES COMPLETOS:', primerProducto._originalData);
        
        console.log('🎨 ANÁLISIS DE COLORES:');
        console.log('   - Colores (IDs):', primerProducto._originalData.colores);
        console.log('   - ColoresNombres:', primerProducto._originalData.coloresNombres);
        console.log('   - ColoresHex:', primerProducto._originalData.coloresHex);
        console.log('   - ColoresDetalle:', primerProducto._originalData.coloresDetalle);
        
        console.log('📏 ANÁLISIS DE TALLES:');
        console.log('   - Talles (IDs):', primerProducto._originalData.talles);
        console.log('   - TallesNombres:', primerProducto._originalData.tallesNombres);
        console.log('   - TallesDetalle:', primerProducto._originalData.tallesDetalle);
        
        // Verificar si hay datos en alguna propiedad
        console.log('🔍 VERIFICACIÓN DE DATOS:');
        console.log('   - ¿ColoresNombres tiene datos?:', 
            primerProducto._originalData.coloresNombres && 
            primerProducto._originalData.coloresNombres.length > 0);
        
        console.log('   - ¿ColoresDetalle tiene datos?:', 
            primerProducto._originalData.coloresDetalle && 
            primerProducto._originalData.coloresDetalle.length > 0);
        
        console.log('   - ¿TallesNombres tiene datos?:', 
            primerProducto._originalData.tallesNombres && 
            primerProducto._originalData.tallesNombres.length > 0);
        
        console.log('   - ¿TallesDetalle tiene datos?:', 
            primerProducto._originalData.tallesDetalle && 
            primerProducto._originalData.tallesDetalle.length > 0);
        
        // Si ColoresDetalle tiene datos, mostrar la estructura
        if (primerProducto._originalData.coloresDetalle && 
            primerProducto._originalData.coloresDetalle.length > 0) {
            console.log('🔍 ESTRUCTURA ColoresDetalle[0]:', primerProducto._originalData.coloresDetalle[0]);
        }
        
        // Si TallesDetalle tiene datos, mostrar la estructura
        if (primerProducto._originalData.tallesDetalle && 
            primerProducto._originalData.tallesDetalle.length > 0) {
            console.log('🔍 ESTRUCTURA TallesDetalle[0]:', primerProducto._originalData.tallesDetalle[0]);
        }
        
    } else {
        console.log('❌ No hay productos cargados');
    }
    
    console.log('=== FIN DIAGNÓSTICO ===');
}


// =============================================
// TRANSFORMACIÓN MEJORADA - USA CUALQUIER DATO DISPONIBLE
// =============================================

function transformProductsToFrontendFormat(productosData) {
    const transformed = {};
    
    console.log('🔍 TRANSFORMACIÓN - Iniciando...');
    
    productosData.forEach((producto, index) => {
        const categoriaReal = producto.categoriaNombre;
        
        if (!categoriaReal) return;
        
        console.log(`🔍 Transformando: "${producto.nombre}"`);
        
        // EXTRAER NOMBRES DE COLORES desde ColoresDetalle
        let coloresNombres = [];
        if (producto.coloresDetalle && Array.isArray(producto.coloresDetalle) && producto.coloresDetalle.length > 0) {
            coloresNombres = producto.coloresDetalle.map(color => color.nombre);
            console.log(`✅ Colores extraídos de ColoresDetalle:`, coloresNombres);
        } else {
            coloresNombres = ['Único'];
            console.log(`❌ Sin datos de colores, usando fallback`);
        }
        
        // EXTRAER NOMBRES DE TALLES desde TallesDetalle
        let tallesNombres = [];
        if (producto.tallesDetalle && Array.isArray(producto.tallesDetalle) && producto.tallesDetalle.length > 0) {
            tallesNombres = producto.tallesDetalle.map(talle => talle.nombre);
            console.log(`✅ Talles extraídos de TallesDetalle:`, tallesNombres);
        } else {
            tallesNombres = ['Único'];
            console.log(`❌ Sin datos de talles, usando fallback`);
        }
        
        if (!transformed[categoriaReal]) {
            transformed[categoriaReal] = [];
        }
        
        transformed[categoriaReal].push({
            id: producto.id,
            name: producto.nombre,
            price: producto.precio,
            image: getImageUrl(producto.imagen),
            images: [getImageUrl(producto.imagen)],
            description: producto.descripcion || 'Descripción no disponible',
            features: getFeaturesFromProduct(producto),
            stock: producto.stock,
            colores: coloresNombres,
            talles: tallesNombres,
            _originalData: producto
        });
    });
    
    return transformed;
}
function getImageUrl(imagenPath) {
    // if (!imagenPath) return '📦';
    
    // Si ya es una URL completa, devolverla tal cual
    if (imagenPath.startsWith('http') || imagenPath.startsWith('//')) {
        return imagenPath;
    }
    
    // Si es una ruta relativa que comienza con /, agregar el backend URL
    if (imagenPath.startsWith('/')) {
        return `${BACKEND_URL}${imagenPath}`;
    }
    
    // Si es solo un nombre de archivo, construir la ruta completa
    // Asumiendo que las imágenes se guardan en una carpeta 'productos'
    return `${BACKEND_URL}/images/productos/${imagenPath}`;
}
function getFeaturesFromProduct(producto) {
    const features = [];
    
    if (producto.stock > 0) {
        features.push('Disponible en stock');
    } else {
        features.push('Próximamente');
    }
    
    // Extraer nombres de colores desde ColoresDetalle
    let coloresNombres = [];
    if (producto.coloresDetalle && producto.coloresDetalle.length > 0) {
        coloresNombres = producto.coloresDetalle.map(color => color.nombre);
    }
    
    // Extraer nombres de talles desde TallesDetalle
    let tallesNombres = [];
    if (producto.tallesDetalle && producto.tallesDetalle.length > 0) {
        tallesNombres = producto.tallesDetalle.map(talle => talle.nombre);
    }
    
    if (coloresNombres.length > 0) {
        if (coloresNombres.length === 1) {
            features.push(`Color: ${coloresNombres[0]}`);
        } else {
            features.push(`${coloresNombres.length} colores disponibles`);
        }
    }
    
    if (tallesNombres.length > 0) {
        if (tallesNombres.length === 1) {
            features.push(`Talle: ${tallesNombres[0]}`);
        } else {
            features.push(`${tallesNombres.length} talles disponibles`);
        }
    }
    
    if (producto.marcaNombre) {
        features.push(`Marca: ${producto.marcaNombre}`);
    }
    
    if (features.length === 0) {
        features.push('Producto premium', 'Calidad garantizada');
    }
    
    return features;
}

// =============================================
// RENDERIZADO DINÁMICO DE CATEGORÍAS
// =============================================
function renderCategories() {
    const categoriesContainer = document.getElementById('categoriesContainer');
    if (!categoriesContainer) {
        console.error('❌ No se encontró el contenedor de categorías con ID "categoriesContainer"');
        return;
    }

    categoriesContainer.innerHTML = '';

    if (categories.length === 0) {
        categoriesContainer.innerHTML = `
            <div class="col-span-full text-center py-12">
                <div class="text-4xl mb-4">😔</div>
                <p class="text-gray-500 text-lg">No hay categorías disponibles</p>
            </div>
        `;
        return;
    }

    categories.forEach(categoria => {
        const categoryCard = document.createElement('div');
        categoryCard.className = 'category-card bg-white rounded-xl shadow-lg p-6 text-center cursor-pointer transition-all duration-300 hover:shadow-xl';
        categoryCard.dataset.category = categoria.nombre;
        categoryCard.innerHTML = `
            <div class="h-20 flex items-center justify-center mb-4 overflow-hidden">
                ${getCategoryImage(categoria)}
            </div>
            <h3 class="text-xl font-semibold text-gray-800 flex items-center justify-center gap-2">
                ${categoria.nombre} <span>🩷</span>
            </h3>
            <p class="text-gray-600 mt-2">${getCategoryDescription(categoria.nombre)}</p>
        `;
        
        categoryCard.addEventListener('click', () => showProducts(categoria.nombre));
        categoriesContainer.appendChild(categoryCard);
    });
}

// =============================================
// NAVEGACIÓN DINÁMICA
// =============================================

function updateNavigationMenu() {
    updateDesktopMenu();
    updateMobileMenu();
    updateFooterMenu();
}
function updateDesktopMenu() {
    const desktopMenu = document.getElementById('desktopProductsMenu');
    if (desktopMenu && categories.length > 0) {
        desktopMenu.innerHTML = categories.map(categoria => {
            return `<a href="#" class="block px-4 py-3 text-gray-700 hover:bg-pink-50 hover:text-pink-600 transition-colors" onclick="showProducts('${categoria.nombre}'); return false;">${categoria.nombre}</a>`;
        }).join('');
    }
}
function updateMobileMenu() {
    const mobileMenu = document.getElementById('mobileProductsMenu');
    if (mobileMenu && categories.length > 0) {
        mobileMenu.innerHTML = categories.map(categoria => {
            return `<a href="#" class="block py-1 text-gray-600 hover:text-pink-600 transition-colors" onclick="showProducts('${categoria.nombre}'); hideMobileMenu(); return false;">${categoria.nombre}</a>`;
        }).join('');
    }
}
function updateFooterMenu() {
    const footerMenu = document.getElementById('footerCategories');
    if (footerMenu && categories.length > 0) {
        footerMenu.innerHTML = categories.map(categoria => {
            return `<a href="#" onclick="showProducts('${categoria.nombre}')" class="block hover:text-pink-400 transition-colors">${categoria.nombre}</a>`;
        }).join('');
    }
}

// =============================================
// FUNCIONES DE LA INTERFAZ PRINCIPAL - OPCIÓN A
// =============================================
function showProducts(category) {
    console.log('🔍 DEBUG - showProducts llamado con categoría:', category);
    console.log('🔍 DEBUG - Estructura de productos PUBLICADOS:', products);
    
    currentCategory = category;
    
    document.getElementById('inicio')?.classList.add('hidden');
    categoriesSection.classList.add('hidden');
    productDetailSection.classList.add('hidden');
    productsSection.classList.remove('hidden');
    
    categoryTitle.textContent = category;
    
    // Verificar SI HAY productos PUBLICADOS en esta categoría
    if (!products[category] || products[category].length === 0) {
        productsGrid.innerHTML = `
            <div class="col-span-full text-center py-12">
                <div class="text-4xl mb-4">😔</div>
                <p class="text-gray-500 text-lg">No hay productos publicados en "${category}"</p>
                <p class="text-gray-400 text-sm mt-2">
                    Los productos aparecen aquí cuando están marcados como "publicados" en el sistema.
                </p>
                <div class="mt-4 space-x-2">
                    <button onclick="goToHome()" class="bg-pink-500 text-white px-6 py-2 rounded-lg hover:bg-pink-600 transition-colors">
                        Volver al inicio
                    </button>
                    <button onclick="recargarProductos()" class="bg-gray-500 text-white px-6 py-2 rounded-lg hover:bg-gray-600 transition-colors">
                        Recargar productos
                    </button>
                </div>
            </div>
        `;
        return;
    }
    
    // Si HAY productos PUBLICADOS, renderizarlos
    renderProductsGrid(category);
    
    setTimeout(() => {
        productsSection.scrollIntoView({ behavior: 'smooth' });
    }, 100);
}
// OPCIÓN A: DOS BOTONES - "AGREGAR" Y "DETALLES"
function renderProductsGrid(category) {
    productsGrid.innerHTML = '';
    
    products[category].forEach(product => {
        const productCard = document.createElement('div');
        productCard.className = 'product-card bg-white rounded-xl shadow-lg overflow-hidden cursor-pointer transition-all duration-300 hover:shadow-xl';
        
        // Usar JSON.stringify y replace para pasar el objeto producto de forma segura
        const productJson = JSON.stringify(product).replace(/"/g, '&quot;');
        
        productCard.innerHTML = `
            <div class="p-6 text-center">
                <div class="h-48 flex items-center justify-center mb-4 overflow-hidden">
                    ${isEmoji(product.image) ? 
                        `<div class="text-6xl">${product.image}</div>` : 
                        `<img src="${product.image}" alt="${product.name}" class="max-h-full max-w-full object-cover rounded-lg" onerror="this.style.display='none'; this.parentElement.innerHTML='<div class=\\'text-6xl\\'>📦</div>';">
                         `}
                </div>
                <h3 class="text-lg font-semibold text-gray-800 mb-2 line-clamp-2">${product.name}</h3>
                <p class="text-2xl font-bold text-pink-600 mb-4">$${product.price}</p>
                <div class="flex space-x-2">
                    <button class="flex-1 bg-pink-500 text-white py-2 rounded-lg hover:bg-pink-600 transition-colors" 
                            onclick="showProductModal(${productJson})">
                        Agregar
                    </button>
                    <button class="flex-1 bg-gray-500 text-white py-2 rounded-lg hover:bg-gray-600 transition-colors" 
                            onclick="showProductDetail(${productJson})">
                        Detalles
                    </button>
                </div>
            </div>
        `;
        productsGrid.appendChild(productCard);
    });
}
function showProductDetail(product) {
    currentProduct = product;
    productsSection.classList.add('hidden');
    productDetailSection.classList.remove('hidden');
    
    document.getElementById('productDetailTitle').textContent = currentCategory;
    document.getElementById('detailProductName').textContent = product.name;
    document.getElementById('detailProductPrice').textContent = `$${product.price}`;
    document.getElementById('detailPrice').textContent = product.price;
    document.getElementById('detailProductDescription').textContent = product.description;
    
    const mainImage = document.getElementById('mainProductImage');
    mainImage.innerHTML = isEmoji(product.image) ? 
        `<div class="text-8xl">${product.image}</div>` :
        `<img src="${product.image}" alt="${product.name}" class="w-full h-full object-cover rounded-lg" onerror="this.style.display='none'; this.parentElement.innerHTML='<div class=\\'text-8xl\\'>📦</div>';">`;
    
    const thumbnailsContainer = document.getElementById('productThumbnails');
    thumbnailsContainer.innerHTML = '';
    
    product.images.forEach((img, index) => {
        const thumbnail = document.createElement('div');
        thumbnail.className = 'w-16 h-16 flex items-center justify-center bg-gray-100 rounded-lg cursor-pointer hover:bg-gray-200 transition-colors overflow-hidden';
        
        if (isEmoji(img)) {
            thumbnail.innerHTML = `<div class="text-2xl">${img}</div>`;
        } else {
            thumbnail.innerHTML = `<img src="${img}" alt="${product.name} ${index + 1}" class="w-full h-full object-cover" onerror="this.style.display='none'; this.parentElement.innerHTML='<div class=\\'text-xl\\'>📦</div>';">`;
        }
        
        thumbnail.addEventListener('click', () => {
            mainImage.innerHTML = isEmoji(img) ? 
                `<div class="text-8xl">${img}</div>` :
                `<img src="${img}" alt="${product.name}" class="w-full h-full object-cover rounded-lg" onerror="this.style.display='none'; this.parentElement.innerHTML='<div class=\\'text-8xl\\'>📦</div>';">`;
        });
        
        thumbnailsContainer.appendChild(thumbnail);
    });
    
    const featuresContainer = document.getElementById('detailProductFeatures');
    featuresContainer.innerHTML = '';
    
    product.features.forEach(feature => {
        const li = document.createElement('li');
        li.className = 'flex items-center text-gray-600';
        li.innerHTML = `<span class="text-pink-500 mr-2">✓</span> ${feature}`;
        featuresContainer.appendChild(li);
    });
    
    updateProductSelects(product);
}

// =============================================
// FUNCIONES PARA MODALES DINÁMICOS
// =============================================

// Función para llenar selects del modal de producto
function updateModalSelects(product) {
    const colorSelect = document.getElementById('colorSelect');
    const sizeSelect = document.getElementById('sizeSelect');

    colorSelect.innerHTML = '<option value="" disabled selected>Selecciona un color</option>';
    sizeSelect.innerHTML = '<option value="" disabled selected>Selecciona un talle</option>';

    // ---- COLORES ----
    if (product.colores && product.colores.length > 0) {
        product.colores.forEach(color => {
            const option = document.createElement('option');
            option.value = color;           
            option.textContent = color;
            option.dataset.nombre = color;
            colorSelect.appendChild(option);
        });
    } else {
        const option = document.createElement('option');
        option.value = "único";
        option.textContent = "Color único";
        option.dataset.nombre = "Color único";
        colorSelect.appendChild(option);
    }

    // ---- TALLES ----
    if (product.talles && product.talles.length > 0) {
        product.talles.forEach(talle => {
            const option = document.createElement('option');
            option.value = talle;
            option.textContent = talle;
            option.dataset.nombre = talle;
            sizeSelect.appendChild(option);
        });
    } else {
        const option = document.createElement('option');
        option.value = "único";
        option.textContent = "Talle único";
        option.dataset.nombre = "Talle único";
        sizeSelect.appendChild(option);
    }
}


// Función para llenar selects del modal de edición
function updateEditModalSelects(product) {
    const colorSelect = document.getElementById('editColorSelect');
    const sizeSelect = document.getElementById('editSizeSelect');
    
    colorSelect.innerHTML = '<option value="" disabled>Selecciona un color</option>';
    sizeSelect.innerHTML = '<option value="" disabled>Selecciona un talle</option>';
    
    // Usar colores REALES del producto
    if (product.colores && product.colores.length > 0) {
        product.colores.forEach(color => {
            const option = document.createElement('option');
            option.value = color;
            option.textContent = color;
            colorSelect.appendChild(option);
        });
    } else {
        const option = document.createElement('option');
        option.value = "único";
        option.textContent = "Color único";
        colorSelect.appendChild(option);
    }
    
    // Usar talles REALES del producto
    if (product.talles && product.talles.length > 0) {
        product.talles.forEach(talle => {
            const option = document.createElement('option');
            option.value = talle;
            option.textContent = talle;
            sizeSelect.appendChild(option);
        });
    } else {
        const option = document.createElement('option');
        option.value = "único";
        option.textContent = "Talle único";
        sizeSelect.appendChild(option);
    }
}
// Mostrar modal de producto para agregar al carrito
function showProductModal(product) {
    currentProduct = product;
    
    document.getElementById('modalProductName').textContent = product.name;
    document.getElementById('modalPrice').textContent = product.price;
    
    updateModalSelects(product);
    
    productModal.classList.remove('hidden');
}
// Cerrar modal de producto
function hideProductModal() {
    productModal.classList.add('hidden');
    currentProduct = null;
}
// FUNCIÓN MEJORADA: Usar colores y talles REALES del producto
// FUNCIÓN MEJORADA: Usar colores y talles REALES del producto
function updateProductSelects(product) {
    const colorSelect = document.getElementById('detailColorSelect');
    const sizeSelect = document.getElementById('detailSizeSelect');

    colorSelect.innerHTML = '';
    sizeSelect.innerHTML = '';

    console.log("🔍 Cargando colores:", product.colores);
    console.log("🔍 Cargando talles:", product.talles);

    // ============================
    // COLORES
    // ============================
    if (product.colores && product.colores.length > 0) {
        product.colores.forEach(color => {
            const option = document.createElement('option');
            option.value = color;       // <-- valor string ("verde")
            option.textContent = color; // <-- texto string
            option.dataset.nombre = color; 
            colorSelect.appendChild(option);
        });
    } else {
        const option = document.createElement('option');
        option.value = "";
        option.textContent = "Color único";
        colorSelect.appendChild(option);
    }

    // ============================
    // TALLES
    // ============================
    if (product.talles && product.talles.length > 0) {
        product.talles.forEach(talle => {
            const option = document.createElement('option');
            option.value = talle;       // <-- string ("S")
            option.textContent = talle;
            option.dataset.nombre = talle;
            sizeSelect.appendChild(option);
        });
    } else {
        const option = document.createElement('option');
        option.value = "";
        option.textContent = "Talle único";
        sizeSelect.appendChild(option);
    }
}



// =============================================
// FUNCIONES DEL CARRITO
// =============================================

function agregarAlCarrito(producto) {
    const cart = JSON.parse(localStorage.getItem('cart')) || [];
    
    const item = {
        id: producto.id, // ← ESTO ES CRÍTICO para que funcione con tu BD
        name: producto.name,
        price: producto.price,
        color: producto.color || producto.selectedColor,
        size: producto.size || producto.selectedSize,
        quantity: producto.quantity || 1,
        image: producto.image
    };
    
    cart.push(item);
    localStorage.setItem('cart', JSON.stringify(cart));
    updateCartCount();
    mostrarNotificacion('¡Éxito!', 'Producto agregado al carrito', 'exito');
}
function addToCart() {
    if (!currentProduct) return;

    const colorSelect = document.getElementById('colorSelect');
    const sizeSelect = document.getElementById('sizeSelect');

    const colorId = colorSelect.value;
    const colorNombre = colorSelect.options[colorSelect.selectedIndex].dataset.nombre;

    const sizeId = sizeSelect.value;
    const sizeNombre = sizeSelect.options[sizeSelect.selectedIndex].dataset.nombre;

    if (!colorId || !sizeId) {
        mostrarNotificacion('Atención', 'Seleccioná color y talle', 'advertencia');
        return;
    }

    const cartItem = {
        id: currentProduct.id,
        name: currentProduct.name,
        price: currentProduct.price,
        image: currentProduct.image,

        color: colorNombre,
        size: sizeNombre,

        quantity: 1,
        cartId: Date.now(),

        availableColors: currentProduct.colores || [],
        availableSizes: currentProduct.talles || []
    };

    cart.push(cartItem);
    updateCartCount();
    mostrarNotificacion('¡Agregado!', `${currentProduct.name} se agregó al carrito`);
    hideProductModal();
}

function addToCartFromDetail() {
    if (!currentProduct) return;

    const colorSelect = document.getElementById('detailColorSelect');
    const sizeSelect = document.getElementById('detailSizeSelect');

    const colorId = colorSelect.value;
    const colorNombre = colorSelect.options[colorSelect.selectedIndex].dataset.nombre;

    const sizeId = sizeSelect.value;
    const sizeNombre = sizeSelect.options[sizeSelect.selectedIndex].dataset.nombre;

    if (!colorId || !sizeId) {
        mostrarNotificacion('Atención', 'Seleccioná color y talle', 'advertencia');
        return;
    }

    const cartItem = {
        id: currentProduct.id,
        name: currentProduct.name,
        price: currentProduct.price,
        image: currentProduct.image,
        color: colorNombre,
        size: sizeNombre,
        quantity: 1,
        cartId: Date.now()
    };

    cart.push(cartItem);
    updateCartCount();
    mostrarNotificacion('¡Agregado!', `${currentProduct.name} se agregó al carrito`);
}


function updateCartCount() {
    const count = cart.length;
    cartCount.textContent = count;
    cartCount.classList.toggle('hidden', count === 0);
}
function showCart() {
    const cartItems = document.getElementById('cartItems');
    const cartTotal = document.getElementById('cartTotal');
    
    if (cart.length === 0) {
        cartItems.innerHTML = '<p class="text-center text-gray-500 py-8">Tu carrito está vacío</p>';
        cartTotal.textContent = '0';
    } else {
        cartItems.innerHTML = '';
        let total = 0;
        
        cart.forEach((item, index) => {
            const itemTotal = item.price * item.quantity;
            total += itemTotal;
            const itemDiv = document.createElement('div');
            itemDiv.className = 'py-4 border-b';
            
            // CORREGIDO: Mostrar imagen correctamente
            const imageHtml = isEmoji(item.image) ? 
                `<div class="text-3xl w-16 h-16 flex items-center justify-center">${item.image}</div>` :
                `<img src="${item.image}" alt="${item.name}" class="w-16 h-16 object-cover rounded" onerror="this.style.display='none'; this.parentElement.innerHTML='<div class=\\'text-3xl w-16 h-16 flex items-center justify-center\\'>📦</div>';">`;
            
            itemDiv.innerHTML = `
                <div class="flex items-start space-x-4">
                    <div class="flex-shrink-0">
                        ${imageHtml}
                    </div>
                    <div class="flex-1">
                        <h4 class="font-semibold text-lg">${item.name}</h4>
                        <div class="flex items-center space-x-4 mt-2">
                            <span class="text-sm text-gray-600">Color: ${item.color}</span>
                            <span class="text-sm text-gray-600">Talle: ${item.size}</span>
                            <button onclick="editCartItem(${index})" class="text-blue-500 hover:text-blue-700 text-sm underline">
                                ✏️ Editar
                            </button>
                        </div>
                        <div class="flex items-center justify-between mt-3">
                            <div class="flex items-center space-x-3">
                                <span class="text-sm text-gray-600">Cantidad:</span>
                                <div class="flex items-center space-x-2">
                                    <button onclick="changeQuantity(${index}, -1)" class="bg-gray-200 hover:bg-gray-300 text-gray-700 w-8 h-8 rounded-full flex items-center justify-center text-sm">
                                        −
                                    </button>
                                    <span class="w-8 text-center font-semibold">${item.quantity}</span>
                                    <button onclick="changeQuantity(${index}, 1)" class="bg-gray-200 hover:bg-gray-300 text-gray-700 w-8 h-8 rounded-full flex items-center justify-center text-sm">
                                        +
                                    </button>
                                </div>
                            </div>
                            <div class="flex items-center space-x-3">
                                <span class="text-pink-600 font-bold">$${itemTotal}</span>
                                <button onclick="removeFromCart(${index})" class="text-red-500 hover:text-red-700 p-1">
                                    🗑️
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            `;
            cartItems.appendChild(itemDiv);
        });
        
        cartTotal.textContent = total;
    }
    
    cartModal.classList.remove('hidden');
}
function hideCart() {
    cartModal.classList.add('hidden');
}
function removeFromCart(index) {
    cart.splice(index, 1);
    updateCartCount();
    showCart();
}
function showCheckout() {
    if (cart.length === 0) {
    mostrarNotificacion('Carrito vacío', 'Agrega productos antes de finalizar la compra', 'advertencia');        return;
    }
    hideCart();
    checkoutModal.classList.remove('hidden');
}
function hideCheckoutModal() {
    checkoutModal.classList.add('hidden');
}
async function processOrder(e) {
    e.preventDefault();
    
    const name = document.getElementById('customerName').value;
    const phone = document.getElementById('customerPhone').value;
    const dni = document.getElementById('customerDNI').value;
    const shippingOption = document.getElementById('shippingOption').value;
    const paymentMethod = document.getElementById('paymentMethod').value;
    
    let address = '';
    let city = '';
    
    if (shippingOption === 'envio') {
        address = document.getElementById('customerAddress').value;
        city = document.getElementById('customerCity').value;
        
        if (!address || !city) {
            mostrarNotificacion('Datos incompletos', 'Por favor completa la dirección y ciudad para el envío', 'advertencia');
            return;
        }
    }
    
    // Validar DNI
    if (!dni) {
        mostrarNotificacion('DNI requerido', 'Por favor ingresa tu DNI', 'advertencia');
        return;
    }

    // Construir objeto de datos del pedido
    const pedidoData = {
        nombre: name,
        telefono: phone,
        dni: dni,
        envio: shippingOption,
        direccion: address,
        ciudad: city,
        metodoPago: paymentMethod,
        productos: cart,
        total: calcularTotal(),
        idPedido: generarIdUnico()
    };

    // 1. Enviar pedido al panel administrativo
    const enviadoAlPanel = await enviarPedidoAlPanel(pedidoData);
    
    if (enviadoAlPanel) {
        // 2. Redirigir a WhatsApp
        redirigirAWhatsApp(pedidoData);
        
        // 3. Limpiar carrito
        cart = [];
        updateCartCount();
        hideCheckoutModal();
        
        // 4. Mostrar notificación de éxito
        mostrarNotificacion('¡Pedido enviado!', 'Te redirigimos a WhatsApp para completar la compra', 'exito');
    } else {
        mostrarNotificacion('Error', 'No se pudo enviar el pedido. Revise la info cargada e intente nuevamente.', 'error');
    }
}


// =============================================
// NUEVAS FUNCIONES PARA ENVÍO DE PEDIDOS AL BACKEND
// =============================================

async function enviarPedidoAlPanel(pedidoData) {
    console.log('🔄 Iniciando envío de pedido al backend...');
    
    try {
        // 1. OBTENER O INICIALIZAR ESTADOS (FUNCIÓN CORREGIDA)
        const estados = await obtenerOInicializarEstados();
        
        if (!estados || estados.length === 0) {
            throw new Error('No hay estados de pedido configurados en el sistema.');
        }

        // 2. BUSCAR ESTADO "PENDIENTE" DE FORMA MÁS FLEXIBLE
        const estadoPedidoId = obtenerEstadoPedidoId(estados);
        console.log(`🎯 Usando estado con ID: ${estadoPedidoId}`);

        // 3. CONSTRUIR PEDIDO PARA BACKEND
        const pedidoParaBackend = construirPedidoBackend(pedidoData, estadoPedidoId);
        console.log("🟦 POST ENVIADO:", JSON.stringify(pedidoParaBackend, null, 2));

        console.log('📤 Enviando al backend:', pedidoParaBackend);

        // 4. ENVIAR AL BACKEND
        const response = await fetch(`${BACKEND_CONFIG.URL}${BACKEND_CONFIG.ENDPOINTS.PEDIDOS}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(pedidoParaBackend)
        });

        // 5. MANEJAR RESPUESTA
        if (response.ok) {
            const result = await response.json();
            console.log('✅ Pedido creado exitosamente en la BD, ID:', result.id);
            return true;
        } else {
            const errorText = await response.text();
            console.error('❌ Error del servidor:', errorText);
            
            let mensajeError = 'No se pudo guardar el pedido';
            if (errorText.includes('EstadoPedidoId') || errorText.includes('estado')) {
                mensajeError = 'Error en los estados de pedido. Contacta al administrador.';
            } else if (errorText.includes('ProductoId')) {
                mensajeError = 'Error en los productos del pedido.';
            } else if (response.status === 400) {
                mensajeError = 'Datos del pedido inválidos. Verifica la información.';
            }
            
            throw new Error(mensajeError);
        }
        
    } catch (error) {
        console.error('❌ Error en enviarPedidoAlPanel:', error);
        mostrarNotificacion('Error', error.message, 'error');
        return false;
    }
}

// =============================================
// FUNCIONES AUXILIARES MEJORADAS
// =============================================

async function obtenerOInicializarEstados() {
    try {
        console.log('🔍 Intentando obtener estados de pedido...');
        
        // PRIMERO: Intentar obtener estados existentes
        const estadosResponse = await fetch(`${BACKEND_CONFIG.URL}${BACKEND_CONFIG.ENDPOINTS.ESTADOS_PEDIDOS}`);
        
        if (estadosResponse.ok) {
            const estados = await estadosResponse.json();
            console.log('✅ Estados obtenidos:', estados);
            return estados;
        }
        
        // SEGUNDO: Si no hay estados, inicializarlos
        console.log('⚠️ No se encontraron estados, inicializando...');
        const initResponse = await fetch(`${BACKEND_CONFIG.URL}${BACKEND_CONFIG.ENDPOINTS.INICIALIZAR_ESTADOS}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            }
        });
        
        if (initResponse.ok || initResponse.status === 409) {
            console.log('✅ Estados inicializados correctamente');
            
            // TERCERO: Obtener los estados recién inicializados
            await new Promise(resolve => setTimeout(resolve, 1000)); // Pequeña pausa
            const nuevosEstadosResponse = await fetch(`${BACKEND_CONFIG.URL}${BACKEND_CONFIG.ENDPOINTS.ESTADOS_PEDIDOS}`);
            
            if (nuevosEstadosResponse.ok) {
                const nuevosEstados = await nuevosEstadosResponse.json();
                console.log('📊 Nuevos estados disponibles:', nuevosEstados);
                return nuevosEstados;
            }
        }
        
        throw new Error('No se pudieron obtener o inicializar los estados de pedido');
        
    } catch (error) {
        console.error('❌ Error en obtenerOInicializarEstados:', error);
        throw error;
    }
}

async function inicializarEstadosPedido() {
    try {
        const response = await fetch(`${BACKEND_CONFIG.URL}${BACKEND_CONFIG.ENDPOINTS.INICIALIZAR_ESTADOS}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            }
        });
        
        if (response.ok) {
            console.log('✅ Estados inicializados correctamente');
            // Esperar y volver a obtener los estados
            await new Promise(resolve => setTimeout(resolve, 1000));
            const nuevosEstadosResponse = await fetch(`${BACKEND_CONFIG.URL}${BACKEND_CONFIG.ENDPOINTS.ESTADOS_PEDIDOS}`);
            return await nuevosEstadosResponse.json();
            
        } else if (response.status === 409) {
            console.log('ℹ️ Los estados ya estaban inicializados');
            // Si ya estaban inicializados, esperar y volver a obtener
            await new Promise(resolve => setTimeout(resolve, 500));
            const nuevosEstadosResponse = await fetch(`${BACKEND_CONFIG.URL}${BACKEND_CONFIG.ENDPOINTS.ESTADOS_PEDIDOS}`);
            return await nuevosEstadosResponse.json();
            
        } else {
            throw new Error('No se pudieron inicializar los estados de pedido');
        }
    } catch (error) {
        console.error('❌ Error inicializando estados:', error);
        throw error;
    }
}

function obtenerEstadoPedidoId(estados) {
    console.log('🔍 Buscando estado adecuado entre:', estados.map(e => `${e.id}: ${e.nombre}`));
    
    // Prioridad de búsqueda de estados
    const estadosPrioritarios = ['pendiente', 'nuevo', 'confirmado', 'recibido'];
    
    for (const estadoNombre of estadosPrioritarios) {
        const estadoEncontrado = estados.find(e => 
            e.nombre && e.nombre.toLowerCase().includes(estadoNombre)
        );
        
        if (estadoEncontrado) {
            console.log(`✅ Estado encontrado: ${estadoEncontrado.nombre} (ID: ${estadoEncontrado.id})`);
            return estadoEncontrado.id;
        }
    }
    
    // Si no encuentra ninguno prioritario, usar el primero
    console.log('⚠️ No se encontró estado prioritario, usando el primero disponible');
    return estados[0].id;
}

// function construirPedidoBackend(pedidoData, estadoPedidoId) {
//     // Validar datos críticos
//     if (!pedidoData.nombre || !pedidoData.dni) {
//         throw new Error('Nombre y DNI del cliente son obligatorios');
//     }

//     return {
//         MontoTotal: pedidoData.total || calcularTotal(pedidoData.productos),
//         Descripcion: `Pedido web - ${pedidoData.idPedido} - Cliente: ${pedidoData.nombre}`,
//         NombreCliente: pedidoData.nombre,
//         DireccionCliente: pedidoData.direccion || 'Retiro en local - Sold. Carrascull - Hernando',
//         Localidad: pedidoData.ciudad || 'Hernando',
//         DNI: parseInt(pedidoData.dni) || 0,
//         Telefono: pedidoData.telefono || '',
//         EstadoPedidoId: estadoPedidoId,
//         PedidoItems: (pedidoData.productos || []).map(item => ({
//             Cantidad: item.quantity || 1,
//             PrecioUnitarioVenta: item.price || 0,
//             ProductoId: item.id
//         }))
//     };
// }


function construirPedidoBackend(pedidoData, estadoPedidoId) {
    if (!pedidoData.nombre || !pedidoData.dni) {
        throw new Error('Nombre y DNI del cliente son obligatorios');
    }

    // === Construcción de descripción compacta ===
    let descripcion = `W#${pedidoData.idPedido.slice(-8)}|${pedidoData.nombre.substring(0, 15)}|`;

    let productosStr = "";
    const maxProductos = 5;

    pedidoData.productos.slice(0, maxProductos).forEach((item, index) => {
        const productoAbrev = item.name.substring(0, 10);
        const colorAbrev = item.color?.substring(0, 3) || 'N/E';
        const talleAbrev = item.size?.substring(0, 3) || 'N/E';

        productosStr += `${productoAbrev}(${colorAbrev},${talleAbrev})x${item.quantity}`;
        if (index < pedidoData.productos.slice(0, maxProductos).length - 1) {
            productosStr += ",";
        }
    });

    if (pedidoData.productos.length > maxProductos) {
        productosStr += `+${pedidoData.productos.length - maxProductos}`;
    }

    descripcion += productosStr;
    descripcion = descripcion.substring(0, 250);

    console.log("🔍 Descripción generada:", descripcion);

    // === RETURN: Pedido para backend ===
    return {
        MontoTotal: pedidoData.total || calcularTotal(pedidoData.productos),
        Descripcion: descripcion,
        NombreCliente: pedidoData.nombre,
        DireccionCliente: pedidoData.direccion || 'Retiro en local',
        Localidad: pedidoData.ciudad || 'Hernando',
        DNI: parseInt(pedidoData.dni) || 0,
        Telefono: pedidoData.telefono || '',
        EstadoPedidoId: estadoPedidoId,

        // 🟣 ACA VA LO IMPORTANTE: COLOR Y TALLE POR ITEM
        PedidoItems: (pedidoData.productos || []).map(item => ({
            Cantidad: item.quantity || 1,
            PrecioUnitarioVenta: item.price || 0,
            ProductoId: item.id,

            // ====== NUEVO ======
            ColorId: item.colorId ?? null,
            ColorNombre: item.color ?? null,
            TalleId: item.talleId ?? null,
            TalleNombre: item.size ?? null
        }))
    };
}


async function manejarRespuestaBackend(response, pedidoData) {
    console.log('📨 Respuesta recibida - Status:', response.status);
    
    if (response.ok) {
        const result = await response.json();
        console.log('✅ Pedido creado exitosamente en la BD, ID:', result.id);
        
        // Redirigir a WhatsApp después de guardar exitosamente
        redirigirAWhatsApp(pedidoData);
        
    } else {
        const errorText = await response.text();
        console.error('❌ Error del servidor:', errorText);
        
        let mensajeError = 'No se pudo guardar el pedido';
        if (errorText.includes('EstadoPedidoId') || errorText.includes('estado')) {
            mensajeError = 'El sistema no tiene estados de pedido configurados. Contacta al administrador.';
        } else if (errorText.includes('ProductoId')) {
            mensajeError = 'Error en los productos del pedido. Verifica que todos los productos existan.';
        } else if (response.status === 400) {
            mensajeError = 'Datos del pedido inválidos. Verifica la información.';
        }
        
        mostrarNotificacion('Error', mensajeError, 'error');
        throw new Error(mensajeError);
    }
}

function calcularTotal(productos) {
    return (productos || []).reduce((total, item) => {
        return total + ((item.price || 0) * (item.quantity || 1));
    }, 0);
}

// =============================================
// FUNCIÓN MEJORADA PARA REDIRIGIR A WHATSAPP
// =============================================

function redirigirAWhatsApp(pedidoData) {
    try {
        const message = construirMensajeWhatsApp(pedidoData);
        const encodedMessage = encodeURIComponent(message);
        const whatsappURL = `https://wa.me/${BACKEND_CONFIG.WHATSAPP}?text=${encodedMessage}`;
        
        console.log('📤 Redirigiendo a WhatsApp...');
        window.open(whatsappURL, '_blank');
        
    } catch (error) {
        console.error('❌ Error al redirigir a WhatsApp:', error);
        mostrarNotificacion('Error', 'No se pudo abrir WhatsApp', 'error');
    }
}

function construirMensajeWhatsApp(pedidoData) {
    let message = `¡Hola! Quiero realizar un pedido:\n\n`;
    
    // Datos del cliente
    message += `*📋 Datos del Cliente:*\n`;
    message += `👤 Nombre: ${pedidoData.nombre || 'No especificado'}\n`;
    message += `🆔 DNI: ${pedidoData.dni || 'No especificado'}\n`;
    message += `📞 Teléfono: ${pedidoData.telefono || 'No especificado'}\n`;
    
    // Información de envío
    message += `\n*🚚 Información de Envío:*\n`;
    if (pedidoData.envio === 'envio') {
        message += `📍 Dirección: ${pedidoData.direccion || 'No especificado'}\n`;
        message += `🏙️ Ciudad: ${pedidoData.ciudad || 'No especificado'}\n`;
    } else {
        message += `🏪 Retiro en local: Sold. Carrascull - Hernando\n`;
    }
    
    // Método de pago
    message += `\n*💳 Método de Pago:*\n`;
    message += `${getPaymentMethodText(pedidoData.metodoPago)}\n`;
    
    // Productos
    message += `\n*🛍️ Productos Solicitados:*\n`;
    let total = 0;
    
    (pedidoData.productos || []).forEach((item, index) => {
        const itemTotal = (item.price || 0) * (item.quantity || 1);
        message += `\n${index + 1}. ${item.name || 'Producto sin nombre'}\n`;
        message += `   🎨 Color: ${item.color || 'No especificado'}\n`;
        message += `   📏 Talle: ${item.size || 'No especificado'}\n`;
        message += `   📦 Cantidad: ${item.quantity || 1}\n`;
        message += `   💰 Precio unitario: $${(item.price || 0).toLocaleString('es-AR')}\n`;
        message += `   🧮 Subtotal: $${itemTotal.toLocaleString('es-AR')}\n`;
        total += itemTotal;
    });
    
    // Resumen financiero
    message += `\n*💰 Resumen del Pedido:*\n`;
    message += `📊 Total: $${total.toLocaleString('es-AR')}\n`;
    
    // Información de transferencia si aplica
    if (pedidoData.metodoPago === 'transferencia') {
        message += `\n*🏦 Datos para Transferencia:*\n`;
        message += `👤 Alias: LourdesGiorda.18\n`;
        message += `💡 Por favor, envía el comprobante de transferencia\n`;
    }
    
    // ID del pedido
    message += `\n*🆔 ID de Pedido:* ${pedidoData.idPedido || 'N/A'}\n\n`;
    message += `¡Gracias por tu compra! 💕\n`;
    message += `Te contactaremos pronto para confirmar tu pedido.`;
    
    return message;
}

// =============================================
// FUNCIÓN DE UTILIDAD PARA FORMATO DE PAGO
// =============================================

function getPaymentMethodText(method) {
    const metodosPago = {
        'transferencia': '💳 Transferencia bancaria',
        'debito': '💳 Tarjeta de débito',
        'credito': '💳 Tarjeta de crédito',
        'efectivo': '💵 Efectivo',
        'mercadopago': '🟡 Mercado Pago'
    };
    
    return metodosPago[method] || method || 'Método no especificado';
}

// =============================================
// FUNCIÓN MEJORADA DE VERIFICACIÓN DE ESTADOS
// =============================================

async function verificarEstadosPedido() {
    try {
        console.log('🔍 Verificando estados de pedido...');
        const response = await fetch(`${BACKEND_CONFIG.URL}${BACKEND_CONFIG.ENDPOINTS.ESTADOS_PEDIDOS}`);
        
        if (response.ok) {
            const estados = await response.json();
            console.log('📊 Estados actuales:', estados);
            
            if (!estados || estados.length === 0) {
                console.log('⚠️ No hay estados de pedido configurados');
                return {
                    exitoso: false,
                    mensaje: 'No hay estados de pedido configurados',
                    estados: []
                };
            }
            
            return {
                exitoso: true,
                mensaje: `Se encontraron ${estados.length} estados de pedido`,
                estados: estados
            };
        } else {
            console.error('❌ Error en la respuesta:', response.status);
            return {
                exitoso: false,
                mensaje: `Error ${response.status} al verificar estados`,
                estados: []
            };
        }
    } catch (error) {
        console.error('❌ Error verificando estados:', error);
        return {
            exitoso: false,
            mensaje: error.message,
            estados: []
        };
    }
}

// Función para generar ID único
function generarIdUnico() {
    return 'PED-' + Date.now() + '-' + Math.random().toString(36).substr(2, 9);
}


function toggleAddressFields() {
    const shippingOption = document.getElementById('shippingOption').value;
    const addressFields = document.getElementById('addressFields');
    const addressInput = document.getElementById('customerAddress');
    const cityInput = document.getElementById('customerCity');
    
    if (shippingOption === 'envio') {
        addressFields.classList.remove('hidden');
        addressInput.required = true;
        cityInput.required = true;
    } else {
        addressFields.classList.add('hidden');
        addressInput.required = false;
        cityInput.required = false;
        addressInput.value = '';
        cityInput.value = '';
    }
}

// =============================================
// FUNCIONES DE EDICIÓN DEL CARRITO (ACTUALIZADAS)
// =============================================

function changeQuantity(index, change) {
    if (cart[index]) {
        cart[index].quantity = Math.max(1, cart[index].quantity + change);
        showCart();
    }
}
function editCartItem(index) {
    if (!cart[index]) return;
    
    editingItemIndex = index;
    const item = cart[index];
    
    // Mostrar imagen en modal de edición
    const editImageElement = document.getElementById('editProductImage');
    if (isEmoji(item.image)) {
        editImageElement.innerHTML = `<div class="text-4xl">${item.image}</div>`;
    } else {
        editImageElement.innerHTML = `<img src="${item.image}" alt="${item.name}" class="w-16 h-16 object-cover rounded" onerror="this.style.display='none'; this.parentElement.innerHTML='<div class=\\'text-4xl\\'>📦</div>';">`;
    }
    
    document.getElementById('editProductName').textContent = item.name;
    document.getElementById('editProductPrice').textContent = `$${item.price}`;
    
    // CORREGIDO: Cargar las opciones disponibles del producto
    loadEditModalOptions(item);
    
    // Establecer los valores actuales
    document.getElementById('editColorSelect').value = item.color;
    document.getElementById('editSizeSelect').value = item.size;
    document.getElementById('editQuantity').textContent = item.quantity;
    
    hideCart();
    editCartModal.classList.remove('hidden');
}

// Nueva función para cargar opciones en el modal de edición
function loadEditModalOptions(item) {
    const colorSelect = document.getElementById('editColorSelect');
    const sizeSelect = document.getElementById('editSizeSelect');
    
    // Limpiar selects
    colorSelect.innerHTML = '';
    sizeSelect.innerHTML = '';
    
    console.log('🔍 Item del carrito para editar:', item); // Para debug
    console.log('🎨 Colores disponibles:', item.availableColors);
    console.log('📏 Talles disponibles:', item.availableSizes);
    
    // Usar las opciones disponibles guardadas en el item del carrito
    const availableColors = item.availableColors || [item.color];
    const availableSizes = item.availableSizes || [item.size];
    
    console.log('✅ Colores a mostrar en select:', availableColors);
    console.log('✅ Talles a mostrar en select:', availableSizes);
    
    // Cargar colores disponibles
    availableColors.forEach(color => {
        const option = document.createElement('option');
        option.value = color;
        option.textContent = color;
        colorSelect.appendChild(option);
    });
    
    // Cargar talles disponibles
    availableSizes.forEach(size => {
        const option = document.createElement('option');
        option.value = size;
        option.textContent = size;
        sizeSelect.appendChild(option);
    });
}
function hideEditModal() {
    editCartModal.classList.add('hidden');
    editingItemIndex = -1;
}
function changeEditQuantity(change) {
    const quantityElement = document.getElementById('editQuantity');
    let currentQuantity = parseInt(quantityElement.textContent);
    currentQuantity = Math.max(1, currentQuantity + change);
    quantityElement.textContent = currentQuantity;
}
function saveCartChanges() {
    if (editingItemIndex === -1 || !cart[editingItemIndex]) return;
    
    const newColor = document.getElementById('editColorSelect').value;
    const newSize = document.getElementById('editSizeSelect').value;
    const newQuantity = parseInt(document.getElementById('editQuantity').textContent);
    
    cart[editingItemIndex].color = newColor;
    cart[editingItemIndex].size = newSize;
    cart[editingItemIndex].quantity = newQuantity;
    
    hideEditModal();
    showCart();
    mostrarNotificacion('¡Actualizado!', 'Producto actualizado correctamente');
}
function viewProductDetails() {
    if (editingItemIndex === -1 || !cart[editingItemIndex]) return;
    
    const item = cart[editingItemIndex];
    let originalProduct = null;
    
    for (const category in products) {
        originalProduct = products[category].find(p => p.id === item.id);
        if (originalProduct) {
            currentCategory = category;
            break;
        }
    }
    
    if (originalProduct) {
        hideEditModal();
        showProductDetail(originalProduct);
    }
}

// =============================================
// FUNCIONES UTILITARIAS
// =============================================

function isEmoji(str) {
    return /^\p{Emoji}$/u.test(str);
}
function getCategoryImage(categoria) {
    if (categoria.imagen && !isEmoji(categoria.imagen)) {
        const emoji = getCategoryEmoji(categoria.nombre);

        return `
            <img 
                src="${getImageUrl(categoria.imagen)}" 
                alt="${categoria.nombre}" 
                class="max-h-full max-w-full object-cover rounded-lg"
                onerror="this.remove(); this.parentElement.textContent='${emoji}';"

            >
        `;
    }

    return getCategoryEmoji(categoria.nombre);

}

function getCategoryEmoji(categoriaNombre) {
    const emojiMap = {
        'Conjuntos': '👙',
        'Corpiños': '👗',
        'Bombachas': '🩲',
        'Pijamas': '🥻',
        'Bikinis': '🏖️',
        'Lencería': '💕',
        'Ropa Interior': '👚',
        'Accesorios': '👜',
        'Calzado': '👠',
        'Deportes': '🏃‍♀️'
    };
    
    if (emojiMap[categoriaNombre]) {
        return emojiMap[categoriaNombre];
    }
    
    const categoriaLower = categoriaNombre.toLowerCase();
    if (categoriaLower.includes('ropa') || categoriaLower.includes('vestimenta')) {
        return '👕';
    } else if (categoriaLower.includes('accesorio')) {
        return '👜';
    } else if (categoriaLower.includes('zapato') || categoriaLower.includes('calzado')) {
        return '👠';
    } else if (categoriaLower.includes('deporte')) {
        return '🏃‍♀️';
    }
    
    return '📦';
}

function getCategoryDescription(categoriaNombre) {
    const specificDescriptions = {
        'Conjuntos': 'Conjuntos completos de lencería',
        'Corpiños': 'Sostenes y corpiños',
        'Bombachas': 'Ropa interior femenina',
        'Pijamas': 'Ropa de dormir cómoda',
        'Bikinis': 'Bikinis y trajes de baño',
        'Lencería': 'Lencería elegante y sensual',
        'Ropa Interior': 'Ropa interior de calidad'
    };
    
    if (specificDescriptions[categoriaNombre]) {
        return specificDescriptions[categoriaNombre];
    }
    
    return `Productos de ${categoriaNombre}`;
}
function scrollToSection(sectionId) {
    const section = document.getElementById(sectionId);
    if (section) {
        section.scrollIntoView({ behavior: 'smooth' });
    }
}
function showCategories() {
    document.getElementById('inicio').classList.remove('hidden');
    categoriesSection.classList.remove('hidden');
    productsSection.classList.add('hidden');
    productDetailSection.classList.add('hidden');
}
function goToHome() {
    document.getElementById('inicio').classList.remove('hidden');
    categoriesSection.classList.remove('hidden');
    productsSection.classList.add('hidden');
    productDetailSection.classList.add('hidden');
    window.scrollTo({ top: 0, behavior: 'smooth' });
}


// FUNCIONES DEL CARRUSEL
// =============================================
function nextSlide() {
    currentSlide = (currentSlide + 1) % totalSlides;
    updateCarousel();
}
function prevSlide() {
    currentSlide = (currentSlide - 1 + totalSlides) % totalSlides;
    updateCarousel();
}
function goToSlide(slideIndex) {
    currentSlide = slideIndex;
    updateCarousel();
}
function updateCarousel() {
    const items = document.querySelectorAll('.carousel-item');
    const indicators = document.querySelectorAll('.carousel-indicator');
    
    items.forEach((item, index) => {
        item.classList.toggle('active', index === currentSlide);
    });
    
    indicators.forEach((indicator, index) => {
        indicator.classList.toggle('active', index === currentSlide);
    });
}
// Auto slide (opcional)
function startAutoSlide() {
    autoSlideInterval = setInterval(nextSlide, 6000); // Cambia cada 6 segundos
}
function stopAutoSlide() {
    clearInterval(autoSlideInterval);
}
// Iniciar auto slide cuando la página carga
document.addEventListener('DOMContentLoaded', function() {
    startAutoSlide();
    
    // Pausar auto slide cuando el usuario interactúa
    const carousel = document.getElementById('heroCarousel');
    carousel.addEventListener('mouseenter', stopAutoSlide);
    carousel.addEventListener('mouseleave', startAutoSlide);
});

// =============================================
// FUNCIONES DEL MENÚ MÓVIL
// =============================================

function toggleMobileMenu() {
    const mobileMenu = document.getElementById('mobileMenu');
    mobileMenu.classList.toggle('hidden');
}

function hideMobileMenu() {
    const mobileMenu = document.getElementById('mobileMenu');
    mobileMenu.classList.add('hidden');
}

function toggleMobileProducts() {
    const mobileProductsMenu = document.getElementById('mobileProductsMenu');
    const mobileProductsIcon = document.getElementById('mobileProductsIcon');
    
    mobileProductsMenu.classList.toggle('hidden');
    mobileProductsIcon.textContent = mobileProductsMenu.classList.contains('hidden') ? '▼' : '▲';
}

// =============================================
// DATOS DE FALLBACK
// =============================================

function getFallbackCategories() {
    return [
        { id: 1, nombre: 'Conjuntos', imagen: '👙' },
        { id: 2, nombre: 'Corpiños', imagen: '👗' },
        { id: 3, nombre: 'Bombachas', imagen: '🩲' },
        { id: 4, nombre: 'Pijamas', imagen: '🥻' }
    ];
}

function getFallbackProducts() {
   return {
        'Conjuntos': [
            {
                id: 1,
                name: "Producto de Ejemplo",
                price: 2500,
                image: "📦",
                images: ["📦"],
                description: "No se pudieron cargar los productos publicados del servidor. Por favor, intenta recargar la página.",
                features: ["Modo offline", "Datos de ejemplo"],
                stock: 0,
                colores: ['Rojo', 'Azul'],
                talles: ['S', 'M']
            }
        ]
    }
}
//debugging functions
// =============================================
// FUNCIONES DE DEPURACIÓN Y DEBUGGING
// =============================================

// Función temporal para debuggear - ejecutar en la consola del navegador
function debugIntegracion() {
    console.log('=== 🐛 DEBUG INTEGRACIÓN COMPLETA ===');
    console.log('1. 📋 CATEGORÍAS del backend:', categories.map(c => c.nombre));
    console.log('2. 📦 PRODUCTOS transformados:', products);
    console.log('3. 🔑 KEYS de products:', Object.keys(products));
    console.log('4. 🎯 Categoría actual:', currentCategory);
    
    if (currentCategory && products[currentCategory]) {
        console.log('5. ✅ PRODUCTOS en categoría actual:', products[currentCategory]);
    } else {
        console.log('5. ❌ NO HAY PRODUCTOS en categoría actual');
    }
    console.log('=== FIN DEBUG ===');
}

// Función para debuggear colores y talles VINCULADOS
function debugColoresTalles() {
    console.log('=== 🎨 DEBUG COLORES Y TALLES VINCULADOS ===');
    
    if (currentProduct) {
        console.log('🔍 Producto actual:', currentProduct.name);
        console.log('🎨 Colores VINCULADOS:', currentProduct.colores);
        console.log('📏 Talles VINCULADOS:', currentProduct.talles);
        console.log('📋 Datos originales del backend:', currentProduct._originalData);
        
        // Verificar estructura de datos del backend
        const original = currentProduct._originalData;
        console.log('🔍 Datos crudos del backend - colores:', original.colores);
        console.log('🔍 Datos crudos del backend - talles:', original.talles);
    } else {
        console.log('❌ No hay producto seleccionado');
    }
    
    console.log('=== FIN DEBUG ===');
}



// Función para verificar la estructura de datos del backend
function verificarEstructuraBackend() {
    console.log('=== 🔍 VERIFICACIÓN ESTRUCTURA BACKEND ===');
    
    if (products && Object.keys(products).length > 0) {
        Object.keys(products).forEach(categoria => {
            console.log(`📂 Categoría: ${categoria}`);
            products[categoria].forEach((producto, index) => {
                console.log(`   📦 Producto ${index + 1}: ${producto.name}`);
                console.log(`      🎨 Colores en backend:`, producto._originalData?.colores);
                console.log(`      📏 Talles en backend:`, producto._originalData?.talles);
                console.log(`      🎨 Colores transformados:`, producto.colores);
                console.log(`      📏 Talles transformados:`, producto.talles);
            });
        });
    } else {
        console.log('❌ No hay productos cargados');
    }
    
    console.log('=== FIN VERIFICACIÓN ===');
}

// =============================================
// FUNCIÓN DE NOTIFICACIÓN ELEGANTE
// =============================================

function mostrarNotificacion(titulo, mensaje, tipo = 'exito') {
    const notificacion = document.getElementById('notificacion');
    const notificacionTitulo = document.getElementById('notificacionTitulo');
    const notificacionMensaje = document.getElementById('notificacionMensaje');
    const notificacionIcono = notificacion.querySelector('.notificacion-icono');
    
    // Configurar según el tipo
    const config = {
        exito: { 
            gradient: 'linear-gradient(135deg, #ec4899, #8b5cf6)',
            icono: '✅'
        },
        error: {
            gradient: 'linear-gradient(135deg, #ef4444, #dc2626)',
            icono: '❌'
        },
        advertencia: {
            gradient: 'linear-gradient(135deg, #f9a8d4, #f472b6)', // ✨ ROSA
            icono: '⚠️'
        }
    };
    
    const { gradient, icono } = config[tipo] || config.exito;
    
    // Aplicar estilos y contenido
    notificacion.style.background = gradient;
    notificacionIcono.textContent = icono;
    notificacionTitulo.textContent = titulo;
    notificacionMensaje.textContent = mensaje;
    
    // Mostrar notificación
    notificacion.classList.add('mostrar');
    
    // Ocultar automáticamente después de 3 segundos
    setTimeout(() => {
        notificacion.classList.remove('mostrar');
    }, 5000);
}

// =============================================
// FUNCIÓN PARA ENVIAR PEDIDOS A .NET
// =============================================


// =============================================
// FUNCIONES GLOBALES
// =============================================
window.verificarEstadosPedido = verificarEstadosPedido;
window.inicializarEstadosPedido = inicializarEstadosPedido;
window.diagnosticoCompletoDTO = diagnosticoCompletoDTO;
window.removeFromCart = removeFromCart;
window.scrollToSection = scrollToSection;
window.changeQuantity = changeQuantity;
window.editCartItem = editCartItem;
window.showProducts = showProducts;
window.hideMobileMenu = hideMobileMenu;
window.goToHome = goToHome;
window.toggleAddressFields = toggleAddressFields;
window.loadProducts = loadProducts;
window.showProductModal = showProductModal;
window.showProductDetail = showProductDetail;
window.hideProductModal = hideProductModal;
window.debugColoresTalles = debugColoresTalles;
window.verificarEstructuraBackend = verificarEstructuraBackend;
window.debugIntegracion = debugIntegracion;
