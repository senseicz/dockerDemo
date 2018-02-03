'use strict';
var express = require('express');
var router = express.Router();

/* return user profile */
router.get('/profile/:id', function (req, res) {
    res.json(
        {
            userId: req.params.id,
            name: 'Mickey Mouse',
            email: 'mickey@disney.com',
            roles: ['mouse', 'CMO']
        }
    );
});

module.exports = router;