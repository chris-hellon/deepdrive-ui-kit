window.tooltip = {
    _initialized: false,

    _init: function () {
        if (this._initialized) return;
        this._initialized = true;

        // Hide tooltip on any click
        document.addEventListener('click', () => this.hide());

        // Hide tooltip on Blazor navigation
        document.addEventListener('enhancedload', () => this.hide());
        
        // Hide tooltip on scroll
        window.addEventListener('scroll', () => this.hide(), true);
    },

    show: function (element, text, position) {
        this._init();
        if (!element || !text) return;

        // Remove any existing tooltip
        this.hide();

        // Create tooltip element
        const tooltip = document.createElement('div');
        tooltip.id = 'dd-tooltip';
        tooltip.className = 'dd-tooltip';
        tooltip.textContent = text;
        document.body.appendChild(tooltip);

        // Get element position
        const rect = element.getBoundingClientRect();
        const tooltipRect = tooltip.getBoundingClientRect();
        const scrollX = window.scrollX || window.pageXOffset;
        const scrollY = window.scrollY || window.pageYOffset;
        const padding = 8;

        let top, left;
        let actualPosition = position || 'top';

        // Auto-detect best position if not specified or if specified position doesn't fit
        if (!position || position === 'auto') {
            const spaceAbove = rect.top;
            const spaceBelow = window.innerHeight - rect.bottom;
            const spaceLeft = rect.left;
            const spaceRight = window.innerWidth - rect.right;

            if (spaceAbove >= tooltipRect.height + padding) {
                actualPosition = 'top';
            } else if (spaceBelow >= tooltipRect.height + padding) {
                actualPosition = 'bottom';
            } else if (spaceRight >= tooltipRect.width + padding) {
                actualPosition = 'right';
            } else if (spaceLeft >= tooltipRect.width + padding) {
                actualPosition = 'left';
            } else {
                actualPosition = 'top'; // fallback
            }
        }

        // Calculate position
        switch (actualPosition) {
            case 'top':
                top = rect.top + scrollY - tooltipRect.height - padding;
                left = rect.left + scrollX + (rect.width / 2) - (tooltipRect.width / 2);
                tooltip.classList.add('dd-tooltip-top');
                break;
            case 'bottom':
                top = rect.bottom + scrollY + padding;
                left = rect.left + scrollX + (rect.width / 2) - (tooltipRect.width / 2);
                tooltip.classList.add('dd-tooltip-bottom');
                break;
            case 'left':
                top = rect.top + scrollY + (rect.height / 2) - (tooltipRect.height / 2);
                left = rect.left + scrollX - tooltipRect.width - padding;
                tooltip.classList.add('dd-tooltip-left');
                break;
            case 'right':
                top = rect.top + scrollY + (rect.height / 2) - (tooltipRect.height / 2);
                left = rect.right + scrollX + padding;
                tooltip.classList.add('dd-tooltip-right');
                break;
        }

        // Keep tooltip within viewport horizontally
        const maxLeft = window.innerWidth + scrollX - tooltipRect.width - padding;
        const minLeft = scrollX + padding;
        left = Math.max(minLeft, Math.min(maxLeft, left));

        // Apply position
        tooltip.style.top = `${top}px`;
        tooltip.style.left = `${left}px`;

        // Trigger animation
        requestAnimationFrame(() => {
            tooltip.classList.add('dd-tooltip-visible');
        });
    },

    hide: function () {
        const tooltip = document.getElementById('dd-tooltip');
        if (tooltip) {
            tooltip.remove();
        }
    }
};
