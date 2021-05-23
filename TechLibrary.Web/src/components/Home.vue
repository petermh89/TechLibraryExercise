<template>
    <div class="home">
        <h1>{{ msg }}</h1>

        <b-row>
            <b-col lg="6" class="my-1">
                <b-form-group labels="Find" label-cols-sm="6" label-align-sm="left" label-for="filterInput" class="mb-0">
                    <b-input-group>
                        <b-form-input v-model="filter"
                                      typeof="search"
                                      placeholder="Search Books">
                        </b-form-input>
                        <b-input-group-append>
                            <b-button :disabled="!filter" class="float-left" @click="filter = ''"> Clear </b-button>
                        </b-input-group-append>
                    </b-input-group>
                </b-form-group>
            </b-col>

            <b-col lg="6" class="my-1">
                <b-pagination v-model="currentPage"
                              :total-rows=400
                              :per-page="perPage"
                              aria-controls="tableOfBooks">

                </b-pagination>
            </b-col>
            <b-col>
                <b-link variant="primary" :to="{name:'addbook'}">Add New Book</b-link>
            </b-col>
        </b-row>

        <b-table id="tableOfBooks" striped hover :items="dataContext" :fields="fields" responsive="sm" :current-page="currentPage" :per-page="perPage" :filter="filter" @filtered="withFilter">
            <template v-slot:cell(thumbnailUrl)="data">
                <b-img :src="data.value" thumbnail fluid></b-img>
            </template>
            <template v-slot:cell(title_link)="data">
                <b-link :to="{ name: 'book_view', params: { 'id' : data.item.bookId } }">{{ data.item.title }}</b-link>
            </template>
        </b-table>

    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'Home',
        props: {
            msg: String
        },
        data: () => ({
            fields: [
                { key: 'thumbnailUrl', label: 'Book Image' },
                { key: 'title_link', label: 'Book Title', sortable: true, sortDirection: 'desc' },
                { key: 'isbn', label: 'ISBN', sortable: true, sortDirection: 'desc' },
                { key: 'descr', label: 'Description', sortable: true, sortDirection: 'desc' }

            ],
            //items: [],
            currentPage: 1,
            perPage: 10,
            totalRows: 0,
            filter: null

        }),

        methods: {
            dataContext(ctx, callback) { //This method processes book data: TODO! possible issues with totalRows param, recuires investigation

                axios.post('https://localhost:5001/books', ctx)
                    .then(response => {
                        this.totalRows = response.data.totalRows;
                        callback(response.data.books);
                    });

            },
            withFilter(filterBy) {
                this.totalRows = filterBy.length;
                this.currentPage = 1
            }
        }
    };
</script>

