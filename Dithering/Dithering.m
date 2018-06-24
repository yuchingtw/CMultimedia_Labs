rgb_I = imread('lena_std.tiff');
gry_I = im2double(rgb2gray(rgb_I));
bw_I = imbinarize(gry_I);


figure(1)
imshow(bw_I);
title("Black White Image");


figure(2)
imshow(gry_I);
title("Gray Image");


figure(3)
imagesc(gry_I, [0, 1])
colormap('default'), colorbar;
title("I scaled and displayed as image");


dither_gry_I1 = zeros(size(gry_I, 1), size(gry_I, 2));
dither_m = [0.1 0.5; 0.7 0.3]; 
% dither_m = (magic(4) - 1) / 16
for x = 1:size(gry_I, 1) 
    for y = 1:size(gry_I, 2)
        i = mod(x, size(dither_m, 2)) + 1;
        j = mod(y, size(dither_m, 1)) + 1;
        dither_gry_I1(x, y) = gry_I(x, y) > dither_m(i, j);
    end
end
figure(4)
imshow(dither_gry_I1);


dither_m2 = fliplr(flipud((magic(4)-ones(4, 4))/16));
for x = 1:size(gry_I, 1)
    for y = 1:size(gry_I, 2)
        for i = 1:size(dither_m2, 1)
            for j = 1:size(dither_m2, 2)
                (x - 1)*size(dither_m2, 2) + i;
                (y - 1)*size(dither_m2, 1) + j;
                dither_gry_I3((x-1)*size(dither_m2, 2) + i, (y-1)*size(dither_m2, 1) + j) = logical(gry_I(x, y)>dither_m2(i, j));
            end
        end
    end
end
figure(5)
imshow(dither_gry_I3);


figure(6)
imhist(gry_I);


figure(7)
imhist(bw_I);