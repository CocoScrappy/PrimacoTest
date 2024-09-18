import React, { useState } from 'react';
import axios from 'axios';
import './App.css'; // Import the CSS file

const SearchComponent = () => {
    const [ticker, setTicker] = useState('');
    const [data, setData] = useState(null);

    const handleSearch = () => {
        if (!ticker) {
            return;
        }
        axios.get(`http://localhost:5000/api/search/${ticker}`)
            .then(response => {
                setData(response.data);
            })
            .catch(error => {
                console.error('Error fetching financial data', error);
            });
    };

    const formatNumber = (number) => {
        const formatter = new Intl.NumberFormat('en-US', {
            style: 'decimal',
            maximumFractionDigits: 2,
        });
        return formatter.format(number);
    };

    return (
        <div className="search-component">
            <label>Enter a ticker symbol:</label>
            <input
                type="text"
                value={ticker}
                onChange={(e) => setTicker(e.target.value)}
                className="ticker-input"
                style={{ width: '100px' }} 
            />
            <button className="btn" onClick={handleSearch}>Search</button>
            {data && (
                <div className="data-display">
                    <p><span>Company:</span> <span className="fldValue">{data.Name}</span></p>
                    <p><span>Market Cap:</span> <span className="fldValue">${formatNumber(data.MarketCap)}</span></p>
                    <p><span>P/E Ratio:</span> <span className="fldValue">{formatNumber(data.PERatio)}</span></p>
                    <p><span>Sector:</span> <span className="fldValue">{data.Sector}</span></p>
                    <p><span>Industry:</span> <span className="fldValue">{data.Industry}</span></p>
                    <p><span>Date:</span> <span className="fldValue">{data.SearchDate}</span></p>
                </div>
            )}
        </div>
    );
};

export default SearchComponent;