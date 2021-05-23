<template>
    <div v-if="book">
        <b-card :title="book.title"
                :img-src="book.thumbnailUrl"
                img-alt="Image"
                img-top
                tag="article"
                style="max-width: 30rem;"
                class="mb-2">
            <div v-if="view">
                <b-card-text>
                    {{ book.descr }}
                </b-card-text>
            </div>
            <div v-else-if="!view">
                <b-form-textarea type="text" v-model="book.descr" rows="3" max-rows="6" :maxlength="1000"></b-form-textarea>
            </div>
            <hr>
            <b-button to="/" variant="primary">Back</b-button>
            <b-button @click="save" variant="success" class="float-right ml-2">Save</b-button>
            <b-button @click="edit" class="float-right">Edit Description</b-button>
        </b-card>
        <p v-if="error">{{ error.message }}</p>
    </div>
</template>

<script>
    import axios from 'axios';
    export default {
        name: 'Book',
        props: ["id"],
        data: () => ({
            book: null,
            error: null,
            description: '',
            view: true,
            file: null,
            baseUrl: 'https://localhost:5001/books',
        }),
        computed: {
            geturl() {
                return `${this.baseUrl}/${this.id}`
            }
        },
        mounted() {
            axios.get(this.geturl)
                .then(response => {
                    this.book = response.data;
                    this.description = this.book.descr;
                });
        },
        methods: { //edit sets view bool to false in order to call else if view for editing purposes
            edit() {
                this.view = false
            },
            save() { // save passes necessary book info to backend and does a simple catch error for updates
                axios.post(
                    `${this.baseUrl}/Update`,
                    {
                        BookId: this.book.bookId,
                        ShortDescr: this.book.descr
                    })
                    .catch(e => {
                        this.updateError(e)
                    })
                    .finally(() => this.view = true)
            },
            updateError(err) {
                this.error = err
            },
        }
    }
</script>