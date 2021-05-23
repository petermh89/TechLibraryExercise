import Vue from 'vue';
import VueRouter from 'vue-router';

Vue.use(VueRouter);

const Home = () => import(/* webpackChunkName: "Home" */ './components/Home.vue');
const Book = () => import(/* webpackChunkName: "Book" */ './components/Book.vue');
const AddNewBook = () => import(/* webpackChunkName: "Book" */ './components/AddNewBook.vue');

const router = new VueRouter({
    routes: [
        { path: '/', component: Home },
        {
            name: 'book_view',
            path: '/book/:id',
            component: Book,
            props: true
        },
        {
            name: 'addbook',
            path: '/add',
            component: AddNewBook,
            props: true
        }
    ]
});

export default router;