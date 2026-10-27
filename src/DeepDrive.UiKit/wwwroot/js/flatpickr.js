// Flatpickr JS Interop for Blazor

const instances = new Map();

export function init(elementId, dotNetRef, options) {
    const element = document.getElementById(elementId);
    if (!element) return;

    // Destroy existing instance if any
    if (instances.has(elementId)) {
        instances.get(elementId).destroy();
        instances.delete(elementId);
    }

    const isTimeOnly = options.noCalendar || false;

    // Get original input classes to apply to altInput
    const originalClasses = element.className || '';

    const config = {
        dateFormat: isTimeOnly ? 'H:i' : 'Y-m-d',
        altInput: !isTimeOnly,
        altFormat: 'd/m/Y',
        altInputClass: originalClasses + ' flatpickr-alt-input',
        enableTime: options.enableTime || false,
        noCalendar: isTimeOnly,
        time_24hr: true,
        minuteIncrement: options.minuteIncrement || 5,
        minDate: options.minDate || null,
        maxDate: options.maxDate || null,
        minTime: options.minTime || null,
        maxTime: options.maxTime || null,
        defaultDate: options.defaultDate || null,
        disableMobile: true,
        allowInput: false,
        clickOpens: true,
        appendTo: document.body,
        onChange: function(selectedDates, dateStr, instance) {
            if (dotNetRef && selectedDates.length > 0) {
                // Use passElementId option to determine callback signature
                if (options.passElementId) {
                    dotNetRef.invokeMethodAsync('OnFlatpickrChange', elementId, dateStr);
                } else {
                    dotNetRef.invokeMethodAsync('OnFlatpickrChange', dateStr);
                }
            }
        },
        onOpen: function(selectedDates, dateStr, instance) {
            // Ensure calendar appears above modals
            instance.calendarContainer.style.zIndex = '2147483647';
        },
        onReady: function(selectedDates, dateStr, instance) {
            // Force width on altInput after Flatpickr creates it
            if (instance.altInput) {
                instance.altInput.style.width = '100%';
                instance.altInput.style.boxSizing = 'border-box';
            }
        }
    };

    const fp = flatpickr(element, config);

    // Also apply styles immediately after init in case onReady didn't fire
    if (fp.altInput) {
        fp.altInput.style.width = '100%';
        fp.altInput.style.boxSizing = 'border-box';
    }

    instances.set(elementId, fp);
}

export function setDate(elementId, date) {
    const fp = instances.get(elementId);
    if (fp && date) {
        fp.setDate(date, false);
    }
}

export function clear(elementId) {
    const fp = instances.get(elementId);
    if (fp) {
        fp.clear();
    }
}

export function destroy(elementId) {
    const fp = instances.get(elementId);
    if (fp) {
        fp.destroy();
        instances.delete(elementId);
    }
}

export function updateOptions(elementId, options) {
    const fp = instances.get(elementId);
    if (fp) {
        if (options.minDate !== undefined) fp.set('minDate', options.minDate);
        if (options.maxDate !== undefined) fp.set('maxDate', options.maxDate);
        // For time constraints, we need to update the config directly and redraw
        if (options.minTime !== undefined) {
            fp.config.minTime = options.minTime;
            fp.set('minTime', options.minTime);
        }
        if (options.maxTime !== undefined) {
            fp.config.maxTime = options.maxTime;
            fp.set('maxTime', options.maxTime);
        }
    }
}
